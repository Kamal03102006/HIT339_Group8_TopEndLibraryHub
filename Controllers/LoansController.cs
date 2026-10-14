using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using TopEndLibraryHub.Services;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.ViewModels;

[Authorize(Roles = "Reception,Admin")]
public class LoansController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CirculationService _circulation;
    private readonly LibraryClock _clock;

    private const decimal DailyFineRate = 1.50m;

    public LoansController(ApplicationDbContext context, CirculationService circulation, LibraryClock clock)
    {
        _context = context;
        _circulation = circulation;
        _clock = clock;
    }

    public async Task<IActionResult> Index(
        string? searchString,
        string? loanStatus)
    {
        var today = _clock.Today;

        var loans = _context.Loans
            .AsNoTracking()
            .Include(loan => loan.Item)
            .Include(loan => loan.Borrower)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            searchString = searchString.Trim();

            loans = loans.Where(loan =>
                loan.Item.LibraryCode.Contains(searchString) ||
                loan.Item.Name.Contains(searchString) ||
                loan.Borrower.MembershipNumber.Contains(searchString) ||
                loan.Borrower.FullName.Contains(searchString));
        }

        loans = loanStatus switch
        {
            "active" => loans.Where(
                loan => loan.ReturnedDate == null),

            "overdue" => loans.Where(
                loan => loan.ReturnedDate == null &&
                        loan.DueDate < today),

            "returned" => loans.Where(
                loan => loan.ReturnedDate != null),

            "unpaid" => loans.Where(
                loan => loan.ReturnedDate != null &&
                        loan.FineAmount > 0 &&
                        !loan.FinePaid),

            _ => loans
        };

        var results = await loans
            .OrderBy(loan => loan.ReturnedDate != null)
            .ThenBy(loan => loan.DueDate)
            .ThenByDescending(loan => loan.BorrowedDate)
            .ToListAsync();

        return View(results);
    }

    [HttpGet]
    public IActionResult Borrow()
    {
        return View(new BorrowItemViewModel { DueDate = _clock.Today.AddDays(14) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(
        BorrowItemViewModel model, CancellationToken cancellationToken = default)
    {
        model.LibraryCode = (model.LibraryCode ?? string.Empty).Trim().ToUpperInvariant();
        model.MembershipNumber = (model.MembershipNumber ?? string.Empty).Trim().ToUpperInvariant();
        ModelState.Clear();
        TryValidateModel(model);
        if (!ModelState.IsValid) return View(model);

        var desk = await GetWorkingDeskAsync(cancellationToken);
        if (desk is null) return RequestWorkingLocation("borrowing");
        var borrowerId = await _context.Borrowers.AsNoTracking()
            .Where(member => member.MembershipNumber == model.MembershipNumber)
            .Select(member => (int?)member.Id).SingleOrDefaultAsync(cancellationToken);
        if (!borrowerId.HasValue)
        {
            ModelState.AddModelError(nameof(model.MembershipNumber),
                "No borrower was found with this membership number.");
            return View(model);
        }

        try
        {
            var result = await _circulation.CheckoutAsync(model.LibraryCode,
                borrowerId.Value, desk.BranchId, model.DueDate, desk.Id, cancellationToken);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(result.Field ?? string.Empty, result.Message);
                return View(model);
            }
            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException)
        {
            _context.ChangeTracker.Clear();
            ModelState.AddModelError(string.Empty,
                "Checkout could not be saved. Refresh the loan list before trying again.");
        }
        catch (SqlException exception) when (exception.Number is 1205 or 1222 or 51001)
        {
            _context.ChangeTracker.Clear();
            ModelState.AddModelError(string.Empty,
                "Another staff action is updating this item. Refresh and try again.");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Return(string? libraryCode)
    {
        ViewData["DailyFineRate"] = DailyFineRate;

        return View(new ReturnItemViewModel
        {
            LibraryCode = libraryCode ?? string.Empty,
            ReturnDate = _clock.Today
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(
        ReturnItemViewModel model, CancellationToken cancellationToken = default)
    {
        ViewData["DailyFineRate"] = DailyFineRate;
        model.LibraryCode = (model.LibraryCode ?? string.Empty).Trim().ToUpperInvariant();
        ModelState.Clear();
        TryValidateModel(model);
        if (!ModelState.IsValid) return View(model);
        var desk = await GetWorkingDeskAsync(cancellationToken);
        if (desk is null) return RequestWorkingLocation("receiving returns");

        try
        {
            var result = await _circulation.ReturnAsync(model.LibraryCode,
                model.ReturnDate, model.PayFineNow, model.NeedsRepair,
                desk.BranchId, desk.Id, cancellationToken);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(result.Field ?? string.Empty, result.Message);
                return View(model);
            }
            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = result.LoanId });
        }
        catch (DbUpdateException)
        {
            _context.ChangeTracker.Clear();
            ModelState.AddModelError(string.Empty,
                "The return could not be saved. Refresh the loan record before retrying.");
        }
        catch (SqlException exception) when (exception.Number is 1205 or 1222 or 51001)
        {
            _context.ChangeTracker.Clear();
            ModelState.AddModelError(string.Empty,
                "Another staff action is updating this item. Refresh and try again.");
        }
        return View(model);
    }

    private async Task<ReceptionDesk?> GetWorkingDeskAsync(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) ||
            HttpContext.Session.GetString("StaffLocationUserId") != userId) return null;
        var branchId = HttpContext.Session.GetInt32("StaffBranchId");
        var deskId = HttpContext.Session.GetInt32("StaffDeskId");
        return await _context.ReceptionDesks.AsNoTracking()
            .SingleOrDefaultAsync(desk => desk.Id == deskId && desk.BranchId == branchId &&
                desk.IsActive && desk.Branch != null && desk.Branch.IsActive,
                cancellationToken);
    }

    private IActionResult RequestWorkingLocation(string operation)
    {
        TempData["LocationMessage"] = $"Select an active branch and reception desk before {operation}.";
        return RedirectToAction("Index", "StaffLocation");
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var loan = await _context.Loans
            .AsNoTracking()
            .Include(existingLoan => existingLoan.Item)
            .Include(existingLoan => existingLoan.Borrower)
            .FirstOrDefaultAsync(existingLoan =>
                existingLoan.Id == id);

        if (loan is null)
        {
            return NotFound();
        }

        return View(loan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PayFine(int id)
    {
        var loan = await _context.Loans
            .Include(existingLoan => existingLoan.Item)
            .FirstOrDefaultAsync(existingLoan =>
                existingLoan.Id == id);

        if (loan is null)
        {
            return NotFound();
        }

        if (loan.ReturnedDate is null || loan.FineAmount <= 0)
        {
            TempData["ErrorMessage"] =
                "This loan does not have a payable fine.";
        }
        else if (loan.FinePaid)
        {
            TempData["ErrorMessage"] =
                "This fine has already been recorded as paid.";
        }
        else
        {
            loan.FinePaid = true;
            loan.FinePaidDate = _clock.UtcNow;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"The {loan.FineAmount:C} fine was recorded as paid.";
        }

        return RedirectToAction(
            nameof(Details),
            new { id });
    }
}