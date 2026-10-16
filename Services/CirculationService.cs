using System.Data;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Services;

public record CirculationResult(bool Succeeded, string Message,
    int? LoanId = null, string? Field = null);

public class CirculationService
{
    private readonly ApplicationDbContext _context;
    private readonly LibraryClock _clock;
    private readonly HoldWorkflowService _holds;
    private readonly NotificationService _notifications;
    private readonly ILogger<CirculationService> _logger;

    public CirculationService(ApplicationDbContext context, LibraryClock clock,
        HoldWorkflowService holds, NotificationService notifications,
        ILogger<CirculationService> logger)
    {
        _context = context;
        _clock = clock;
        _holds = holds;
        _notifications = notifications;
        _logger = logger;
    }

    // Caller resolves the member from staff input or the authenticated Identity link.
    // A kiosk never supplies someone else's membership number to this method.
    public async Task<CirculationResult> CheckoutAsync(string libraryCode,
        int borrowerId, int branchId, DateTime dueDate, int? deskId = null,
        CancellationToken cancellationToken = default)
    {
        var code = libraryCode.Trim().ToUpperInvariant();
        var itemId = await _context.Items.AsNoTracking()
            .Where(item => item.LibraryCode == code).Select(item => (int?)item.Id)
            .SingleOrDefaultAsync(cancellationToken);
        if (!itemId.HasValue)
            return new(false, "No item was found with this library code.", Field: "LibraryCode");
        if (dueDate.Date <= _clock.Today)
            return new(false, "The due date must be after today.", Field: "DueDate");

        await using var transaction = await _context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable, cancellationToken);
        await _holds.LockItemAsync(itemId.Value, cancellationToken);
        var item = await _context.Items.SingleOrDefaultAsync(
            item => item.Id == itemId && item.LibraryCode == code, cancellationToken);
        if (item is null)
            return new(false, "The item record changed. Refresh and scan it again.", Field: "LibraryCode");

        if (!await ValidLocationAsync(branchId, deskId, cancellationToken))
            return new(false, "Choose an active operating branch and reception desk.");
        var borrower = await _context.Borrowers.SingleOrDefaultAsync(
            member => member.Id == borrowerId, cancellationToken);
        if (borrower is null || !borrower.IsActive)
            return new(false, "An active library membership is required.", Field: "MembershipNumber");
        if (item.BranchId != branchId)
            return new(false, "This item is at another branch. Arrange its transfer first.", Field: "LibraryCode");
        if (item.Status == ItemStatus.InTransit ||
            await _context.ItemTransfers.AnyAsync(transfer => transfer.ItemId == item.Id &&
                transfer.Status == TransferStatus.InTransit, cancellationToken))
            return new(false, "The destination branch must receive this item before checkout.", Field: "LibraryCode");
        if (item.Status != ItemStatus.Available ||
            await _context.Loans.AnyAsync(loan => loan.ItemId == item.Id &&
                loan.ReturnedDate == null, cancellationToken))
            return new(false, "This item is not currently available for borrowing.", Field: "LibraryCode");

        var hold = await _holds.RefreshLockedAsync(item, cancellationToken);
        if (hold is not null && hold.BorrowerId != borrower.Id)
            return new(false, "This item is reserved for the next member in its waiting list.", Field: "LibraryCode");
        if (hold is not null && (hold.Status != HoldStatus.ReadyForPickup ||
            hold.PickupBranchId != branchId))
            return new(false, "Your reserved item must reach its pickup branch before checkout.", Field: "LibraryCode");

        var loan = new Loan
        {
            ItemId = item.Id,
            BorrowerId = borrower.Id,
            BorrowedDate = _clock.UtcNow,
            DueDate = dueDate.Date,
            FineAmount = 0,
            FinePaid = false
        };
        _context.Loans.Add(loan);
        item.Status = ItemStatus.Borrowed;
        if (hold is not null)
        {
            hold.Status = HoldStatus.Fulfilled;
            hold.FulfilledAtUtc = _clock.UtcNow;
        }
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        try
        {
            await _notifications.RecordBorrowedAsync(loan.Id, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception,
                "Borrowing simulation for loan {LoanId} will be retried by the worker.", loan.Id);
        }

        return new(true, $"'{item.Name}' was borrowed by {borrower.FullName}. " +
            $"Due {loan.DueDate:dd MMM yyyy}.", loan.Id);
    }

    public async Task<CirculationResult> ReturnAsync(string libraryCode,
        DateTime returnDate, bool payFineNow, bool needsRepair, int branchId, int deskId,
        CancellationToken cancellationToken = default)
    {
        var code = libraryCode.Trim().ToUpperInvariant();
        var itemId = await _context.Items.AsNoTracking()
            .Where(item => item.LibraryCode == code).Select(item => (int?)item.Id)
            .SingleOrDefaultAsync(cancellationToken);
        if (!itemId.HasValue)
            return new(false, "No item was found with this library code.", Field: "LibraryCode");
        if (returnDate.Date > _clock.Today)
            return new(false, "The return date cannot be in the future.", Field: "ReturnDate");

        await using var transaction = await _context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable, cancellationToken);
        await _holds.LockItemAsync(itemId.Value, cancellationToken);
        if (!await ValidLocationAsync(branchId, deskId, cancellationToken))
            return new(false, "Select an active branch and reception desk before receiving a return.");
        var loan = await _context.Loans.Include(loan => loan.Item)
            .SingleOrDefaultAsync(loan => loan.ItemId == itemId &&
                loan.Item.LibraryCode == code && loan.ReturnedDate == null,
                cancellationToken);
        if (loan is null)
            return new(false, "No active loan was found. An already returned item is not changed.", Field: "LibraryCode");
        if (returnDate.Date < _clock.ToDarwinTime(loan.BorrowedDate).Date)
            return new(false, "The return date cannot be before the borrowing date.", Field: "ReturnDate");
        if (loan.Item.Status == ItemStatus.InTransit ||
            await _context.ItemTransfers.AnyAsync(transfer => transfer.ItemId == itemId &&
                transfer.Status == TransferStatus.InTransit, cancellationToken))
            return new(false, "An in-transit transfer cannot be received as a loan return.", Field: "LibraryCode");

        FinePolicy.RecordReturn(loan, returnDate, payFineNow, _clock.UtcNow);
        // Due/return inputs are Darwin calendar dates; event timestamps are UTC.
        loan.ReturnedDate = returnDate.Date == _clock.Today ? _clock.UtcNow :
            new DateTimeOffset(DateTime.SpecifyKind(returnDate.Date.AddDays(1).AddTicks(-1),
                DateTimeKind.Unspecified), TimeSpan.FromMinutes(570)).UtcDateTime;
        loan.Item.BranchId = branchId;
        loan.Item.Status = needsRepair ? ItemStatus.Damaged : ItemStatus.Available;
        await _context.SaveChangesAsync(cancellationToken);
        var nextHold = await _holds.RefreshLockedAsync(loan.Item, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        await _holds.NotifyReadyAfterCommitAsync(nextHold, cancellationToken);

        var condition = needsRepair ? " It is awaiting repair." :
            nextHold is null ? string.Empty : " The next eligible request has been allocated.";
        var fine = loan.FineAmount > 0 ? $" Late fine: AUD ${loan.FineAmount:0.00}." : " No late fine.";
        return new(true, $"'{loan.Item.Name}' was returned." + fine + condition, loan.Id);
    }

    private async Task<bool> ValidLocationAsync(int branchId, int? deskId,
        CancellationToken cancellationToken)
    {
        if (deskId.HasValue)
            return await _context.ReceptionDesks.AnyAsync(desk => desk.Id == deskId &&
                desk.BranchId == branchId && desk.IsActive && desk.Branch != null &&
                desk.Branch.IsActive, cancellationToken);
        return await _context.Branches.AnyAsync(branch => branch.Id == branchId &&
            branch.IsActive, cancellationToken);
    }
}
