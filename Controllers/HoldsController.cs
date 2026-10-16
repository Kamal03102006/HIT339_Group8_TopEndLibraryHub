using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers;

[Authorize(Roles = "Admin,Reception")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class HoldsController : Controller
{
    private const int PageSize = 25;
    private readonly ApplicationDbContext _context;
    private readonly LibraryClock _clock;
    private readonly HoldWorkflowService _workflow;
    private readonly HoldPlacementService _placement;

    public HoldsController(ApplicationDbContext context, LibraryClock clock, HoldWorkflowService workflow,
        HoldPlacementService placement)
    {
        _context = context;
        _clock = clock;
        _workflow = workflow;
        _placement = placement;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? search, HoldStatus? status, int page = 1,
        CancellationToken cancellationToken = default)
    {
        search = search?.Trim();
        if (search is { Length: > 100 })
        {
            ModelState.AddModelError(nameof(search),
                "Search must be 100 characters or fewer.");
            search = search[..100];
        }

        if (status.HasValue && !Enum.IsDefined(typeof(HoldStatus), status.Value))
        {
            ModelState.AddModelError(nameof(status), "Choose a valid request status.");
            status = null;
        }

        var query = _context.Holds.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(hold =>
                hold.Item.LibraryCode.Contains(search) ||
                hold.Item.Name.Contains(search) ||
                hold.Borrower.MembershipNumber.Contains(search) ||
                hold.Borrower.FullName.Contains(search));
        }

        if (status.HasValue)
        {
            query = query.Where(hold => hold.Status == status.Value);
        }

        var total = await query.CountAsync(cancellationToken);
        var pageCount = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        page = Math.Clamp(page, 1, pageCount);

        var rows = await query
            .OrderByDescending(hold => hold.CreatedAtUtc)
            .ThenByDescending(hold => hold.Id)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(hold => new HoldListRow
            {
                Id = hold.Id,
                LibraryCode = hold.Item.LibraryCode,
                ItemName = hold.Item.Name,
                CurrentItemStatus = hold.Item.Status,
                MembershipNumber = hold.Borrower.MembershipNumber,
                MemberName = hold.Borrower.FullName,
                PickupBranchName = hold.PickupBranch.Name,
                MemberIsActive = hold.Borrower.IsActive,
                PickupBranchIsActive = hold.PickupBranch.IsActive,
                Status = hold.Status,
                CreatedAtUtc = hold.CreatedAtUtc,
                QueuePosition = hold.Status == HoldStatus.Waiting &&
                    hold.Borrower.IsActive && hold.PickupBranch.IsActive
                    ? (int?)_context.Holds.Count(waiting =>
                        waiting.ItemId == hold.ItemId &&
                        waiting.Status == HoldStatus.Waiting &&
                        waiting.Borrower.IsActive && waiting.PickupBranch.IsActive &&
                        (waiting.CreatedAtUtc < hold.CreatedAtUtc ||
                         (waiting.CreatedAtUtc == hold.CreatedAtUtc &&
                          waiting.Id <= hold.Id)))
                    : null
            })
            .ToListAsync(cancellationToken);

        foreach (var row in rows)
        {
            row.CreatedAtDarwin = _clock.ToDarwinTime(row.CreatedAtUtc);
        }

        return View(new HoldListViewModel
        {
            Search = search,
            Status = status,
            Page = page,
            PageCount = pageCount,
            TotalCount = total,
            Rows = rows
        });
    }

    [HttpGet]
    public async Task<IActionResult> Create(
        string? libraryCode, CancellationToken cancellationToken = default)
    {
        var model = new CreateHoldViewModel
        {
            LibraryCode = (libraryCode ?? string.Empty).Trim().ToUpperInvariant()
        };
        await PopulateBranchesAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateHoldViewModel model, CancellationToken cancellationToken = default)
    {
        model.LibraryCode = (model.LibraryCode ?? string.Empty).Trim().ToUpperInvariant();
        model.MembershipNumber =
            (model.MembershipNumber ?? string.Empty).Trim().ToUpperInvariant();
        ModelState.Clear();
        TryValidateModel(model);

        if (!ModelState.IsValid)
        {
            await PopulateBranchesAsync(model, cancellationToken);
            return View(model);
        }

        var result = await _placement.PlaceForStaffAsync(model.LibraryCode,
            model.MembershipNumber, model.PickupBranchId!.Value, cancellationToken);
        if (result.Succeeded)
        {
            TempData["HoldSuccess"] = result.Message;
            return RedirectToAction(nameof(Index), new { search = model.LibraryCode });
        }
        foreach (var error in result.Errors)
            ModelState.AddModelError(error.Field, error.Message);
        await PopulateBranchesAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(
        int id, CancellationToken cancellationToken = default)
    {
        var itemId = await _context.Holds.AsNoTracking()
            .Where(hold => hold.Id == id).Select(hold => (int?)hold.ItemId)
            .SingleOrDefaultAsync(cancellationToken);
        if (!itemId.HasValue) return NotFound();

        await using var transaction = await _context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable, cancellationToken);
        try
        {
            await _workflow.LockItemAsync(itemId.Value, cancellationToken);
            var hold = await _context.Holds.Include(hold => hold.Item)
                .SingleOrDefaultAsync(hold => hold.Id == id, cancellationToken);
            if (hold is null) return NotFound();
            if (hold.Status is not (HoldStatus.Waiting or HoldStatus.Allocated or HoldStatus.ReadyForPickup))
            {
                TempData["HoldError"] = "This request is already cancelled or collected. No changes were made.";
                return RedirectToAction(nameof(Index));
            }
            if (hold.Status != HoldStatus.Waiting &&
                await _context.Loans.AnyAsync(loan => loan.ItemId == hold.ItemId &&
                    loan.ReturnedDate == null, cancellationToken))
            {
                TempData["HoldError"] = "The item has an active loan. Ask staff to review the request first.";
                return RedirectToAction(nameof(Index));
            }

            hold.Status = HoldStatus.Cancelled;
            hold.CancelledAtUtc = _clock.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            var allocation = await _workflow.RefreshLockedAsync(hold.Item, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            await _workflow.NotifyReadyAfterCommitAsync(allocation, cancellationToken);
            TempData["HoldSuccess"] = "Request cancelled; its history is retained.";
            return RedirectToAction(nameof(Index), new { search = hold.Item.LibraryCode });
        }
        catch (DbUpdateException)
        {
            await transaction.DisposeAsync();
            _context.ChangeTracker.Clear();
            TempData["HoldError"] = "The request changed during cancellation. Refresh and check its status.";
        }
        catch (SqlException exception) when (exception.Number is 1205 or 1222 or 51001)
        {
            await transaction.DisposeAsync();
            _context.ChangeTracker.Clear();
            TempData["HoldError"] = "Another staff action is updating this item. Refresh before trying again.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateBranchesAsync(
        CreateHoldViewModel model, CancellationToken cancellationToken)
    {
        model.PickupBranches = await _context.Branches.AsNoTracking()
            .Where(branch => branch.IsActive)
            .OrderBy(branch => branch.Name)
            .Select(branch => new SelectListItem
            {
                Value = branch.Id.ToString(),
                Text = branch.Name
            })
            .ToListAsync(cancellationToken);
    }
}
