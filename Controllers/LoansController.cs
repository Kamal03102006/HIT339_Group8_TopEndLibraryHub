using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.ViewModels;

[Authorize(Roles = "Reception,Admin")]
public class LoansController : Controller
{
    private readonly ApplicationDbContext _context;

    private const decimal DailyFineRate = 1.50m;

    public LoansController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Displays active, overdue and returned loans.
    public async Task<IActionResult> Index(
        string? searchString,
        string? loanStatus)
    {
        var today = DateTime.UtcNow.Date;

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

    public IActionResult Borrow()
    {
        return View(new BorrowItemViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(
        BorrowItemViewModel model)
    {
        model.LibraryCode =
            model.LibraryCode.Trim().ToUpperInvariant();

        model.MembershipNumber =
            model.MembershipNumber.Trim().ToUpperInvariant();

        ModelState.Clear();
        TryValidateModel(model);

        var today = DateTime.UtcNow.Date;

        if (model.DueDate.Date <= today)
        {
            ModelState.AddModelError(
                nameof(model.DueDate),
                "The due date must be after today.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var item = await _context.Items.FirstOrDefaultAsync(
            item => item.LibraryCode == model.LibraryCode);

        var borrower = await _context.Borrowers.FirstOrDefaultAsync(
            borrower =>
                borrower.MembershipNumber == model.MembershipNumber);

        if (item is null)
        {
            ModelState.AddModelError(
                nameof(model.LibraryCode),
                "No item was found with this library code.");
        }
        else
        {
            var alreadyOnLoan = await _context.Loans.AnyAsync(
                loan => loan.ItemId == item.Id &&
                        loan.ReturnedDate == null);

            if (item.Status != ItemStatus.Available || alreadyOnLoan)
            {
                ModelState.AddModelError(
                    nameof(model.LibraryCode),
                    "This item is not currently available for borrowing.");
            }
        }

        if (borrower is null)
        {
            ModelState.AddModelError(
                nameof(model.MembershipNumber),
                "No borrower was found with this membership number.");
        }
        else if (!borrower.IsActive)
        {
            ModelState.AddModelError(
                nameof(model.MembershipNumber),
                "This borrower’s membership is inactive.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var loan = new Loan
        {
            ItemId = item!.Id,
            BorrowerId = borrower!.Id,
            BorrowedDate = DateTime.UtcNow,
            DueDate = model.DueDate.Date,
            FineAmount = 0,
            FinePaid = false
        };

        item.Status = ItemStatus.Borrowed;

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            $"'{item.Name}' was borrowed by {borrower.FullName}. " +
            $"It is due on {loan.DueDate:dd MMMM yyyy}.";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Return(string? libraryCode)
    {
        ViewData["DailyFineRate"] = DailyFineRate;

        return View(new ReturnItemViewModel
        {
            LibraryCode = libraryCode ?? string.Empty,
            ReturnDate = DateTime.UtcNow.Date
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(
        ReturnItemViewModel model)
    {
        ViewData["DailyFineRate"] = DailyFineRate;

        model.LibraryCode =
            model.LibraryCode.Trim().ToUpperInvariant();

        ModelState.Clear();
        TryValidateModel(model);

        var today = DateTime.UtcNow.Date;

        if (model.ReturnDate.Date > today)
        {
            ModelState.AddModelError(
                nameof(model.ReturnDate),
                "The return date cannot be in the future.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var loan = await _context.Loans
            .Include(existingLoan => existingLoan.Item)
            .Include(existingLoan => existingLoan.Borrower)
            .FirstOrDefaultAsync(existingLoan =>
                existingLoan.Item.LibraryCode == model.LibraryCode &&
                existingLoan.ReturnedDate == null);

        if (loan is null)
        {
            ModelState.AddModelError(
                nameof(model.LibraryCode),
                "No active loan was found for this library code.");

            return View(model);
        }

        if (model.ReturnDate.Date < loan.BorrowedDate.Date)
        {
            ModelState.AddModelError(
                nameof(model.ReturnDate),
                "The return date cannot be before the borrowing date.");

            return View(model);
        }

        var overdueDays = Math.Max(
            0,
            (model.ReturnDate.Date - loan.DueDate.Date).Days);

        loan.FineAmount = overdueDays * DailyFineRate;

        loan.ReturnedDate = model.ReturnDate.Date
            .AddDays(1)
            .AddTicks(-1);

        loan.FinePaid =
            loan.FineAmount == 0 || model.PayFineNow;

        loan.FinePaidDate =
            loan.FineAmount > 0 && model.PayFineNow
                ? DateTime.UtcNow
                : null;

        loan.Item.Status = ItemStatus.Available;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            loan.FineAmount > 0
                ? $"'{loan.Item.Name}' was returned. " +
                  $"A late fine of {loan.FineAmount:C} was applied."
                : $"'{loan.Item.Name}' was returned with no late fine.";

        return RedirectToAction(
            nameof(Details),
            new { id = loan.Id });
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
            loan.FinePaidDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"The {loan.FineAmount:C} fine was recorded as paid.";
        }

        return RedirectToAction(
            nameof(Details),
            new { id });
    }
}