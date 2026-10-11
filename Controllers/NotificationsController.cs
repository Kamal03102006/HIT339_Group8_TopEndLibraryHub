using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers
{
    [Authorize(Roles = "Admin,Reception,Manager")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NotificationService _notifications;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(
            ApplicationDbContext context,
            NotificationService notifications,
            ILogger<NotificationsController> logger)
        {
            _context = context;
            _notifications = notifications;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            NotificationKind? kind = null,
            NotificationChannel? channel = null,
            string? search = null,
            int page = 1,
            CancellationToken cancellationToken = default)
        {
            search = search?.Trim().ToUpperInvariant() ?? string.Empty;
            if (!ModelState.IsValid || search.Length > 20 ||
                (kind.HasValue && !Enum.IsDefined(kind.Value)) ||
                (channel.HasValue && !Enum.IsDefined(channel.Value)))
            {
                return BadRequest("Choose valid filters and a search code of at most 20 characters.");
            }

            var query = _context.NotificationLogs.AsNoTracking();
            if (kind.HasValue)
            {
                query = query.Where(record => record.Kind == kind.Value);
            }
            if (channel.HasValue)
            {
                query = query.Where(record => record.Channel == channel.Value);
            }
            if (search.Length > 0)
            {
                query = query.Where(record =>
                    record.Borrower.MembershipNumber.Contains(search) ||
                    (record.Loan != null && record.Loan.Item.LibraryCode.Contains(search)) ||
                    (record.Hold != null && record.Hold.Item.LibraryCode.Contains(search)));
            }

            const int pageSize = 25;
            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = Math.Max(1,
                (int)Math.Ceiling(totalRecords / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            // Project only the fields needed for the staff page. The second
            // sort key keeps paging deterministic when timestamps match.
            var records = await query
                .OrderByDescending(record => record.CreatedAtUtc)
                .ThenByDescending(record => record.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(record => new NotificationRecordViewModel
                {
                    CreatedAtUtc = record.CreatedAtUtc,
                    Kind = record.Kind,
                    Channel = record.Channel,
                    MembershipNumber = record.Borrower.MembershipNumber,
                    BorrowerName = record.Borrower.FullName,
                    ItemCode = record.Loan != null ? record.Loan.Item.LibraryCode :
                        record.Hold != null ? record.Hold.Item.LibraryCode : string.Empty,
                    ItemName = record.Loan != null ? record.Loan.Item.Name :
                        record.Hold != null ? record.Hold.Item.Name : string.Empty,
                    Recipient = record.Recipient,
                    Message = record.Message
                })
                .ToListAsync(cancellationToken);

            return View(new NotificationDashboardViewModel
            {
                Kind = kind,
                Channel = channel,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = records
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckNow(CancellationToken cancellationToken)
        {
            try
            {
                var created = await _notifications.ProcessActiveLoansAsync(cancellationToken);
                TempData["NotificationSuccess"] = created == 0
                    ? "Check complete. No new records were needed."
                    : $"Check complete. {created} new simulated records were created.";
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "A staff notification check failed.");
                TempData["NotificationError"] =
                    "The check could not finish. Records already created remain saved. " +
                    "Automatic checks will retry while the app is running.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
