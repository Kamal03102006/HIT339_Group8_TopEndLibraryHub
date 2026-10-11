using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly LibraryClock _clock;
        private const decimal DailyFineRate = 1.50m;

        public NotificationService(ApplicationDbContext context, LibraryClock clock)
        {
            _context = context;
            _clock = clock;
        }

        public async Task<int> RecordBorrowedAsync(
            int loanId, CancellationToken cancellationToken = default)
        {
            var loan = await _context.Loans.AsNoTracking()
                .Include(loan => loan.Borrower)
                .Include(loan => loan.Item)
                .FirstOrDefaultAsync(loan => loan.Id == loanId, cancellationToken);

            if (loan is null || loan.ReturnedDate.HasValue || !loan.Borrower.IsActive)
            {
                return 0;
            }

            return await WriteBorrowedAsync(loan, _clock.UtcNow, cancellationToken);
        }

        // Called by the background worker. Borrow notices are also caught up
        // here if another module creates a loan without calling the method above.
        public async Task<int> ProcessActiveLoansAsync(
            CancellationToken cancellationToken = default)
        {
            var utcNow = _clock.UtcNow;
            var today = _clock.ToDarwinTime(utcNow).Date;
            var loans = await _context.Loans.AsNoTracking()
                .Include(loan => loan.Borrower)
                .Include(loan => loan.Item)
                .Where(loan => loan.ReturnedDate == null && loan.Borrower.IsActive)
                .OrderBy(loan => loan.Id)
                .ToListAsync(cancellationToken);

            var created = 0;
            foreach (var loan in loans)
            {
                cancellationToken.ThrowIfCancellationRequested();
                created += await WriteBorrowedAsync(loan, utcNow, cancellationToken);
                created += await WriteReminderAsync(loan, today, utcNow, cancellationToken);
            }

            return created;
        }

        public async Task<int> RecordHoldReadyAsync(
            int holdId, string pickupBranchName,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(pickupBranchName);

            var hold = await _context.Holds.AsNoTracking()
                .Include(hold => hold.Borrower)
                .Include(hold => hold.Item)
                .FirstOrDefaultAsync(hold => hold.Id == holdId, cancellationToken);

            if (hold is null || hold.Status != HoldStatus.ReadyForPickup ||
                !hold.ReadyForPickupAtUtc.HasValue || !hold.Borrower.IsActive)
            {
                return 0;
            }

            var message = $"'{hold.Item.Name}' ({hold.Item.LibraryCode}) is ready " +
                $"for pickup at {pickupBranchName.Trim()}. " +
                "Please bring your library membership card.";

            return await WriteBothChannelsAsync(hold.Borrower, null, hold.Id, null,
                NotificationKind.HoldReady, $"hold:{hold.Id}:ready", message,
                _clock.UtcNow, cancellationToken);
        }

        private Task<int> WriteBorrowedAsync(
            Loan loan, DateTime utcNow, CancellationToken cancellationToken)
        {
            var message = $"Loan confirmed: '{loan.Item.Name}' " +
                $"({loan.Item.LibraryCode}). Due {DisplayDate(loan.DueDate)}.";

            return WriteBothChannelsAsync(loan.Borrower, loan.Id, null, loan.DueDate,
                NotificationKind.Borrowed, $"loan:{loan.Id}:borrowed", message,
                utcNow, cancellationToken);
        }

        private Task<int> WriteReminderAsync(
            Loan loan, DateTime today, DateTime utcNow,
            CancellationToken cancellationToken)
        {
            var daysRemaining = (loan.DueDate.Date - today).Days;

            if (daysRemaining >= 0 && daysRemaining <= 2)
            {
                var when = daysRemaining switch
                {
                    0 => "today",
                    1 => "tomorrow",
                    _ => "in two days"
                };
                var message = $"Reminder: '{loan.Item.Name}' " +
                    $"({loan.Item.LibraryCode}) is due {when} " +
                    $"({DisplayDate(loan.DueDate)}).";

                // One approaching-due reminder per loan and due date.
                return WriteBothChannelsAsync(loan.Borrower, loan.Id, null, loan.DueDate,
                    NotificationKind.DueSoon, $"loan:{loan.Id}:due:{DateKey(loan.DueDate)}",
                    message, utcNow, cancellationToken);
            }

            if (daysRemaining < 0)
            {
                var overdueDays = -daysRemaining;
                var fine = (overdueDays * DailyFineRate)
                    .ToString("0.00", CultureInfo.InvariantCulture);
                var dayLabel = overdueDays == 1 ? "day" : "days";
                var message = $"'{loan.Item.Name}' ({loan.Item.LibraryCode}) is " +
                    $"{overdueDays} {dayLabel} overdue. Estimated fine: AUD ${fine} " +
                    $"as at {DisplayDate(today)}. Return the item to stop further daily charges.";

                // Accruing fines are logged once per Darwin business date.
                return WriteBothChannelsAsync(loan.Borrower, loan.Id, null, loan.DueDate,
                    NotificationKind.FineAccruing, $"loan:{loan.Id}:fine:{DateKey(today)}",
                    message, utcNow, cancellationToken);
            }

            return Task.FromResult(0);
        }

        private async Task<int> WriteBothChannelsAsync(
            Borrower borrower, int? loanId, int? holdId, DateTime? expectedDueDate,
            NotificationKind kind, string eventKey, string message, DateTime utcNow,
            CancellationToken cancellationToken)
        {
            var created = 0;
            if (!string.IsNullOrWhiteSpace(borrower.Email))
            {
                created += await WriteOnceAsync(borrower.Id, loanId, holdId, expectedDueDate,
                    NotificationChannel.Email, kind, eventKey, borrower.Email.Trim(),
                    message, utcNow, cancellationToken);
            }

            if (!string.IsNullOrWhiteSpace(borrower.Phone))
            {
                created += await WriteOnceAsync(borrower.Id, loanId, holdId, expectedDueDate,
                    NotificationChannel.Sms, kind, eventKey, borrower.Phone.Trim(),
                    message, utcNow, cancellationToken);
            }

            return created;
        }

        private Task<int> WriteOnceAsync(
            int borrowerId, int? loanId, int? holdId, DateTime? expectedDueDate,
            NotificationChannel channel, NotificationKind kind, string eventKey,
            string recipient, string message, DateTime utcNow,
            CancellationToken cancellationToken)
        {
            var channelValue = (int)channel;
            var kindValue = (int)kind;

            // The lookup and insert share one SQL statement. Range locks and
            // the unique event/channel index protect simultaneous checks.
            // Values are parameters, including messages containing apostrophes.
            // Recheck source state so a stale scan cannot notify a returned loan
            // or a hold that has already been cancelled or collected.
            return _context.Database.ExecuteSqlInterpolatedAsync($@"
                SET NOCOUNT OFF;
                INSERT INTO [NotificationLogs]
                    ([BorrowerId], [LoanId], [HoldId], [Channel], [Kind],
                     [EventKey], [Recipient], [Message], [CreatedAtUtc])
                SELECT {borrowerId}, {loanId}, {holdId}, {channelValue}, {kindValue},
                       {eventKey}, {recipient}, {message}, {utcNow}
                WHERE NOT EXISTS (
                    SELECT 1 FROM [NotificationLogs] WITH (UPDLOCK, HOLDLOCK)
                    WHERE [EventKey] = {eventKey} AND [Channel] = {channelValue}
                )
                AND EXISTS (
                    SELECT 1 FROM [Borrowers] WITH (HOLDLOCK)
                    WHERE [Id] = {borrowerId} AND [IsActive] = 1
                )
                AND (
                    EXISTS (
                        SELECT 1 FROM [Loans] WITH (HOLDLOCK)
                        WHERE [Id] = {loanId} AND [BorrowerId] = {borrowerId}
                          AND [DueDate] = {expectedDueDate} AND [ReturnedDate] IS NULL
                    )
                    OR EXISTS (
                        SELECT 1 FROM [Holds] WITH (HOLDLOCK)
                        WHERE [Id] = {holdId} AND [BorrowerId] = {borrowerId}
                          AND [Status] = 2 AND [ReadyForPickupAtUtc] IS NOT NULL
                    )
                );", cancellationToken);
        }

        private static string DateKey(DateTime date) =>
            date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        private static string DisplayDate(DateTime date) =>
            date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
    }
}
