using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers;

[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class KioskController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _users;
    private readonly SignInManager<IdentityUser> _signIn;
    private readonly IConfiguration _configuration;
    private readonly CirculationService _circulation;
    private readonly LibraryClock _clock;
    private readonly HoldPlacementService _placement;

    public KioskController(ApplicationDbContext db, UserManager<IdentityUser> users,
        SignInManager<IdentityUser> signIn, IConfiguration configuration,
        CirculationService circulation, LibraryClock clock, HoldPlacementService placement)
    {
        _db = db; _users = users; _signIn = signIn; _configuration = configuration;
        _circulation = circulation; _clock = clock; _placement = placement;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Index(string? search, string? status, string? type,
        bool expired = false)
    {
        var query = _db.Items.AsNoTracking().AsQueryable();
        search = search?.Trim() ?? string.Empty;
        if (search.Length > 100) search = search[..100];
        if (search.Length > 0)
            query = query.Where(item => item.Name.Contains(search) ||
                item.LibraryCode.Contains(search) || item.Description.Contains(search));

        var reservedIds = CirculationRules.ReservedItemIds(_db);
        if (status == "OnHold") query = query.Where(item => reservedIds.Contains(item.Id));
        else if (Enum.TryParse<ItemStatus>(status, true, out var parsed) && Enum.IsDefined(parsed))
            query = parsed == ItemStatus.Available ? CirculationRules.GenerallyAvailable(_db, query) :
                query.Where(item => item.Status == parsed);
        else status = string.Empty;

        if (type == "Book") query = query.OfType<Book>();
        else if (type == "Toy") query = query.OfType<Toy>();
        else if (type == "Music") query = query.OfType<Music>();
        else type = string.Empty;

        var items = await query.Include(item => item.Branch).OrderBy(item => item.Name)
            .ThenBy(item => item.Id).Take(100).ToListAsync();
        var resultIds = items.Select(item => item.Id).ToArray();
        var availableIds = CirculationRules.GenerallyAvailable(_db).Select(item => item.Id);
        ViewBag.Expired = expired;
        return View(new CatalogueViewModel
        {
            SearchString = search, SelectedStatus = status ?? string.Empty,
            SelectedType = type ?? string.Empty, Items = items,
            AvailableItemIds = (await availableIds.Where(id => resultIds.Contains(id)).ToListAsync()).ToHashSet(),
            ReservedItemIds = (await reservedIds.Where(id => resultIds.Contains(id)).ToListAsync()).ToHashSet()
        });
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Scan() => View(await BuildScanAsync(null));

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Scan(string? libraryCode)
    {
        var code = (libraryCode ?? string.Empty).Trim().ToUpperInvariant();
        if (!ValidCode(code))
        {
            var invalid = await BuildScanAsync(null);
            invalid.Message = "Enter a valid item code, such as BK-0001.";
            return View(invalid);
        }
        return View(await BuildScanAsync(code));
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Account()
    {
        var userId = _users.GetUserId(User);
        if (userId is null) return Challenge();
        var borrower = await _db.Borrowers.AsNoTracking()
            .SingleOrDefaultAsync(member => member.IdentityUserId == userId);
        var vm = new KioskAccountViewModel();
        if (borrower is null)
        {
            vm.Message = "This account is not linked to a library membership. Ask authorised staff for help.";
            return View(vm);
        }

        vm.FullName = borrower.FullName;
        vm.MembershipNumber = borrower.MembershipNumber;
        vm.IsActive = borrower.IsActive;
        if (!borrower.IsActive) vm.Message = "Your membership is inactive. Borrowing and new pickup requests are unavailable.";
        vm.Loans = await _db.Loans.AsNoTracking()
            .Where(loan => loan.BorrowerId == borrower.Id && loan.ReturnedDate == null)
            .OrderBy(loan => loan.DueDate).Select(loan => new KioskLoanRow
            {
                Name = loan.Item.Name, LibraryCode = loan.Item.LibraryCode,
                DueDate = loan.DueDate, FineAmount = loan.FineAmount
            }).ToListAsync();
        foreach (var loan in vm.Loans)
            loan.EstimatedLateFine = Math.Max(0, (_clock.Today - loan.DueDate.Date).Days) * 1.50m;
        vm.EstimatedLateFines = vm.Loans.Sum(loan => loan.EstimatedLateFine);
        vm.OutstandingFines = await _db.Loans.AsNoTracking()
            .Where(loan => loan.BorrowerId == borrower.Id && !loan.FinePaid).SumAsync(loan => loan.FineAmount);

        vm.Holds = await _db.Holds.AsNoTracking()
            .Where(hold => hold.BorrowerId == borrower.Id &&
                (hold.Status == HoldStatus.Waiting || hold.Status == HoldStatus.Allocated ||
                 hold.Status == HoldStatus.ReadyForPickup))
            .OrderBy(hold => hold.CreatedAtUtc).ThenBy(hold => hold.Id)
            .Select(hold => new KioskHoldRow
            {
                Name = hold.Item.Name, LibraryCode = hold.Item.LibraryCode,
                PickupBranch = hold.PickupBranch.Name, Status = hold.Status,
                EligibilityPaused = !hold.Borrower.IsActive || !hold.PickupBranch.IsActive,
                QueuePosition = hold.Status == HoldStatus.Waiting && hold.Borrower.IsActive &&
                    hold.PickupBranch.IsActive ? _db.Holds.Count(earlier =>
                        earlier.ItemId == hold.ItemId && earlier.Status == HoldStatus.Waiting &&
                        earlier.Borrower.IsActive && earlier.PickupBranch.IsActive &&
                        (earlier.CreatedAtUtc < hold.CreatedAtUtc ||
                         (earlier.CreatedAtUtc == hold.CreatedAtUtc && earlier.Id <= hold.Id))) : (int?)null
            }).ToListAsync();
        return View(vm);
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Hold(string? libraryCode,
        CancellationToken cancellationToken = default)
    {
        var userId = _users.GetUserId(User);
        if (userId is null) return Challenge();
        var borrower = await _db.Borrowers.AsNoTracking().SingleOrDefaultAsync(
            member => member.IdentityUserId == userId, cancellationToken);
        if (borrower is null || !borrower.IsActive)
        {
            TempData["KioskError"] = "An active membership linked to your login is required to place a hold.";
            return RedirectToAction(nameof(Account));
        }
        var model = new KioskHoldViewModel
        {
            LibraryCode = (libraryCode ?? string.Empty).Trim().ToUpperInvariant()
        };
        if (model.LibraryCode.Length > 0 && !ValidCode(model.LibraryCode))
        {
            model.LibraryCode = string.Empty;
            ModelState.AddModelError(nameof(model.LibraryCode), "Enter a valid item code, such as BK-0001.");
        }
        await PopulateHoldFormAsync(model, borrower, cancellationToken);
        return View(model);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Hold([FromForm] KioskHoldViewModel model,
        CancellationToken cancellationToken = default)
    {
        var userId = _users.GetUserId(User);
        if (userId is null) return Challenge();
        var borrower = await _db.Borrowers.AsNoTracking().SingleOrDefaultAsync(
            member => member.IdentityUserId == userId, cancellationToken);
        if (borrower is null || !borrower.IsActive)
        {
            TempData["KioskError"] = "An active membership linked to your login is required to place a hold.";
            return RedirectToAction(nameof(Account));
        }

        model.LibraryCode = (model.LibraryCode ?? string.Empty).Trim().ToUpperInvariant();
        ModelState.Clear();
        TryValidateModel(model);
        if (ModelState.IsValid)
        {
            // The service rechecks this Identity link and eligibility inside the
            // item transaction. No posted membership or borrower ID is used.
            var result = await _placement.PlaceForPatronAsync(model.LibraryCode,
                userId, model.PickupBranchId!.Value, cancellationToken);
            if (result.Succeeded)
            {
                TempData["KioskSuccess"] = result.Message;
                return RedirectToAction(nameof(Account));
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Field, error.Message);
        }
        await PopulateHoldFormAsync(model, borrower, cancellationToken);
        return View(model);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(string? libraryCode)
    {
        var userId = _users.GetUserId(User);
        if (userId is null) return Challenge();
        // Ownership is derived only from the signed-in Identity user.
        var borrower = await _db.Borrowers.AsNoTracking()
            .SingleOrDefaultAsync(member => member.IdentityUserId == userId);
        if (borrower is null || !borrower.IsActive)
        {
            TempData["KioskError"] = "An active membership linked to your login is required.";
            return RedirectToAction(nameof(Account));
        }
        var code = (libraryCode ?? string.Empty).Trim().ToUpperInvariant();
        if (!ValidCode(code))
        {
            TempData["KioskError"] = "Enter a valid library item code.";
            return RedirectToAction(nameof(Scan));
        }
        var branch = await KioskBranchAsync();
        if (branch is null)
        {
            TempData["KioskError"] = "This kiosk's operating branch is unavailable. Ask staff for help.";
            return RedirectToAction(nameof(Account));
        }

        try
        {
            var result = await _circulation.CheckoutAsync(code, borrower.Id, branch.Id,
                _clock.Today.AddDays(14), cancellationToken: HttpContext.RequestAborted);
            TempData[result.Succeeded ? "KioskSuccess" : "KioskError"] = result.Message;
        }
        catch (DbUpdateException)
        {
            TempData["KioskError"] = "The item changed during checkout. Check your account before trying again.";
        }
        catch (SqlException exception) when (exception.Number is 1205 or 1222 or 51001)
        {
            TempData["KioskError"] = "This item is being updated. Check your account, then try again.";
        }
        return RedirectToAction(nameof(Account));
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public IActionResult KeepAlive() => NoContent();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Finish()
    {
        await _signIn.SignOutAsync();
        HttpContext.Session.Clear();
        Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
        Response.Headers.Pragma = "no-cache";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateHoldFormAsync(KioskHoldViewModel model, Borrower borrower,
        CancellationToken cancellationToken)
    {
        model.MemberName = borrower.FullName;
        model.MembershipNumber = borrower.MembershipNumber;
        model.PickupBranches = await _db.Branches.AsNoTracking()
            .Where(branch => branch.IsActive).OrderBy(branch => branch.Name)
            .Select(branch => new SelectListItem
            {
                Value = branch.Id.ToString(), Text = branch.Name
            }).ToListAsync(cancellationToken);
        var item = ValidCode(model.LibraryCode)
            ? await _db.Items.AsNoTracking().SingleOrDefaultAsync(
                item => item.LibraryCode == model.LibraryCode, cancellationToken) : null;
        model.ItemName = item?.Name;
        model.ItemStatus = item?.Status;
    }

    private Task<Branch?> KioskBranchAsync()
    {
        // Trusted deployment setting, never an editable patron form field.
        var branchCode = (_configuration["Kiosk:BranchCode"] ?? "DAR").Trim().ToUpperInvariant();
        return _db.Branches.AsNoTracking().SingleOrDefaultAsync(branch =>
            branch.BranchCode == branchCode && branch.IsActive);
    }

    private async Task<KioskScanViewModel> BuildScanAsync(string? code)
    {
        var branch = await KioskBranchAsync();
        var userId = _users.GetUserId(User);
        var borrower = await _db.Borrowers.AsNoTracking()
            .SingleOrDefaultAsync(member => member.IdentityUserId == userId);
        var vm = new KioskScanViewModel
        {
            KioskBranch = branch?.Name ?? "Unavailable", LibraryCode = code ?? string.Empty
        };
        if (borrower is null || !borrower.IsActive)
            vm.Message = "An active membership linked to your login is required for checkout.";
        if (code is null) return vm;
        var item = await _db.Items.AsNoTracking().Include(item => item.Branch)
            .SingleOrDefaultAsync(item => item.LibraryCode == code);
        vm.Item = item;
        if (item is null) { vm.Message = "No item was found for that code."; return vm; }
        var generallyAvailable = await CirculationRules.GenerallyAvailable(_db)
            .AnyAsync(candidate => candidate.Id == item.Id);
        var reserved = await CirculationRules.ReservedItemIds(_db).ContainsAsync(item.Id);
        vm.Availability = generallyAvailable ? "Available" : item.Status == ItemStatus.InTransit ?
            "In transit" : reserved && item.Status == ItemStatus.Available ? "On hold" : item.Status.ToString();
        if (borrower is null || !borrower.IsActive || branch is null || item.BranchId != branch.Id) return vm;
        var ownReadyHold = item.Status == ItemStatus.Available &&
            await _db.Holds.AnyAsync(hold => hold.ItemId == item.Id &&
                hold.BorrowerId == borrower.Id && hold.Status == HoldStatus.ReadyForPickup &&
                hold.PickupBranchId == branch.Id &&
                !_db.Loans.Any(loan => loan.ItemId == hold.ItemId && loan.ReturnedDate == null) &&
                !_db.ItemTransfers.Any(transfer => transfer.ItemId == hold.ItemId &&
                    transfer.Status == TransferStatus.InTransit));
        vm.CanCheckout = generallyAvailable || ownReadyHold;
        if (ownReadyHold) vm.Availability = "Ready for your collection";
        return vm;
    }

    private static bool ValidCode(string code) => code.Length <= 20 &&
        Regex.IsMatch(code, "^[A-Z]{2,4}-[0-9]{4}$");
}
