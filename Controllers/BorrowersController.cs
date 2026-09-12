using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

[Authorize(Roles = "Reception,Admin")]
public class BorrowersController : Controller
{
    private readonly ApplicationDbContext _context;

    public BorrowersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Displays borrowers with searching and account-status filtering.
    public async Task<IActionResult> Index(
        string? searchString,
        string? activity)
    {
        var borrowers = _context.Borrowers
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            searchString = searchString.Trim();

            borrowers = borrowers.Where(b =>
                b.MembershipNumber.Contains(searchString) ||
                b.FullName.Contains(searchString) ||
                b.Email.Contains(searchString) ||
                b.Phone.Contains(searchString));
        }

        if (activity == "active")
        {
            borrowers = borrowers.Where(b => b.IsActive);
        }
        else if (activity == "inactive")
        {
            borrowers = borrowers.Where(b => !b.IsActive);
        }

        return View(await borrowers
            .OrderBy(b => b.FullName)
            .ToListAsync());
    }

    // Displays borrower details and their borrowing history.
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrower = await _context.Borrowers
            .AsNoTracking()
            .Include(b => b.Loans)
            .ThenInclude(l => l.Item)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrower is null)
        {
            return NotFound();
        }

        return View(borrower);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("MembershipNumber,FullName,Email,Phone,Address")]
        Borrower borrower)
    {
        borrower.MembershipNumber =
            borrower.MembershipNumber.Trim().ToUpperInvariant();

        borrower.Email = borrower.Email.Trim();

        if (await MembershipNumberExistsAsync(borrower.MembershipNumber))
        {
            ModelState.AddModelError(
                nameof(Borrower.MembershipNumber),
                "This membership number is already being used.");
        }

        if (await EmailExistsAsync(borrower.Email))
        {
            ModelState.AddModelError(
                nameof(Borrower.Email),
                "This email address is already registered.");
        }

        if (!ModelState.IsValid)
        {
            return View(borrower);
        }

        borrower.DateRegistered = DateTime.UtcNow;
        borrower.IsActive = true;

        try
        {
            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Borrower '{borrower.FullName}' was created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                string.Empty,
                "The borrower could not be saved. Check that the membership number and email are unique.");

            return View(borrower);
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrower = await _context.Borrowers
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrower is null)
        {
            return NotFound();
        }

        return View(borrower);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,MembershipNumber,FullName,Email,Phone,Address,IsActive")]
        Borrower borrower)
    {
        if (id != borrower.Id)
        {
            return NotFound();
        }

        var existingBorrower = await _context.Borrowers
            .Include(b => b.Loans)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (existingBorrower is null)
        {
            return NotFound();
        }

        borrower.MembershipNumber =
            borrower.MembershipNumber.Trim().ToUpperInvariant();

        borrower.Email = borrower.Email.Trim();
        borrower.DateRegistered = existingBorrower.DateRegistered;

        if (await MembershipNumberExistsAsync(
                borrower.MembershipNumber,
                borrower.Id))
        {
            ModelState.AddModelError(
                nameof(Borrower.MembershipNumber),
                "This membership number is already being used.");
        }

        if (await EmailExistsAsync(borrower.Email, borrower.Id))
        {
            ModelState.AddModelError(
                nameof(Borrower.Email),
                "This email address is already registered.");
        }

        var hasActiveLoan = existingBorrower.Loans.Any(
            loan => loan.ReturnedDate == null);

        if (!borrower.IsActive && hasActiveLoan)
        {
            ModelState.AddModelError(
                nameof(Borrower.IsActive),
                "A borrower with an active loan cannot be deactivated.");
        }

        if (!ModelState.IsValid)
        {
            return View(borrower);
        }

        existingBorrower.MembershipNumber = borrower.MembershipNumber;
        existingBorrower.FullName = borrower.FullName;
        existingBorrower.Email = borrower.Email;
        existingBorrower.Phone = borrower.Phone;
        existingBorrower.Address = borrower.Address;
        existingBorrower.IsActive = borrower.IsActive;

        try
        {
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Borrower '{borrower.FullName}' was updated successfully.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await BorrowerExistsAsync(borrower.Id))
            {
                return NotFound();
            }

            ModelState.AddModelError(
                string.Empty,
                "This borrower was changed by another user. Reload the page and try again.");

            return View(borrower);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                string.Empty,
                "The borrower could not be updated. Check the entered information.");

            return View(borrower);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrower = await _context.Borrowers
            .AsNoTracking()
            .Include(b => b.Loans)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrower is null)
        {
            return NotFound();
        }

        return View(borrower);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var borrower = await _context.Borrowers
            .Include(b => b.Loans)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrower is null)
        {
            return NotFound();
        }

        if (borrower.Loans.Any())
        {
            TempData["ErrorMessage"] =
                "This borrower cannot be deleted because borrowing history must be retained.";

            return RedirectToAction(nameof(Index));
        }

        _context.Borrowers.Remove(borrower);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            $"Borrower '{borrower.FullName}' was deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    private Task<bool> BorrowerExistsAsync(int id)
    {
        return _context.Borrowers.AnyAsync(b => b.Id == id);
    }

    private Task<bool> MembershipNumberExistsAsync(
        string membershipNumber,
        int? excludedId = null)
    {
        return _context.Borrowers.AnyAsync(b =>
            b.MembershipNumber == membershipNumber &&
            (!excludedId.HasValue || b.Id != excludedId.Value));
    }

    private Task<bool> EmailExistsAsync(
        string email,
        int? excludedId = null)
    {
        return _context.Borrowers.AnyAsync(b =>
            b.Email == email &&
            (!excludedId.HasValue || b.Id != excludedId.Value));
    }
}