using System.Data;
using Microsoft.Data.SqlClient;
using TopEndLibraryHub.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HoldWorkflowService _holds;

        public BooksController(ApplicationDbContext context, HoldWorkflowService holds)
        {
            _context = context;
            _holds = holds;
        }

        public async Task<IActionResult> Index(
            string? searchTerm,
            ItemStatus? status)
        {
            var books = _context.Books
                .AsNoTracking()
                .Include(book => book.Branch)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                books = books.Where(book =>
                    book.Name.Contains(searchTerm) ||
                    book.LibraryCode.Contains(searchTerm) ||
                    book.Author.Contains(searchTerm) ||
                    book.BookGenre.Contains(searchTerm));
            }

            if (status.HasValue)
            {
                books = books.Where(book => book.Status == status);
            }

            ViewData["CurrentSearch"] = searchTerm;
            ViewData["CurrentStatus"] = status;

            return View(await books
                .OrderBy(book => book.Name)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .AsNoTracking()
                .Include(book => book.Branch)
                .FirstOrDefaultAsync(book => book.Id == id);

            return book is null ? NotFound() : View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var desk = await GetWorkingDeskAsync();

            if (desk is null)
            {
                return RequestWorkingLocation();
            }

            return View(new Book
            {
                BranchId = desk.BranchId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Author,BookGenre,Isbn,PublicationYear,Publisher," +
                "LibraryCode,Name,Description")]
            Book book)
        {
            var desk = await GetWorkingDeskAsync();

            if (desk is null)
            {
                return RequestWorkingLocation();
            }

            // Assign the branch from the validated staff location.
            // Ignore any branch value submitted by the browser.
            book.BranchId = desk.BranchId;
            ModelState.Remove(nameof(Book.BranchId));

            if (await LibraryCodeExistsAsync(book.LibraryCode))
            {
                ModelState.AddModelError(
                    nameof(Book.LibraryCode),
                    "This library code is already being used.");
            }

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            book.Status = ItemStatus.Available;
            book.DateAdded = DateTime.UtcNow;

            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Book '{book.Name}' was created at {desk.Branch!.Name}.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The book could not be saved. Check that its " +
                    "library code is unique and its branch still exists.");

                return View(book);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .AsNoTracking()
                .Include(book => book.Branch)
                .FirstOrDefaultAsync(book => book.Id == id);

            return book is null ? NotFound() : View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind(
                "Author,BookGenre,Isbn,PublicationYear,Publisher," +
                "LibraryCode,Name,Description,Status")]
            Book input)
        {
            if (id <= 0) return NotFound();
            await using var transaction = await _context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable);
            try
            {
                await _holds.LockItemAsync(id);
            }
            catch (SqlException exception) when (exception.Number is 1205 or 1222 or 51001)
            {
                TempData["ErrorMessage"] = "This item is being updated. Refresh before editing it.";
                return RedirectToAction(nameof(Index));
            }

            var book = await _context.Books
                .Include(book => book.Branch)
                .FirstOrDefaultAsync(book => book.Id == id);

            if (book is null)
            {
                return NotFound();
            }

            // Ordinary edits cannot change the physical location.
            input.BranchId = book.BranchId;
            input.Branch = book.Branch;
            input.Id = book.Id;
            input.DateAdded = book.DateAdded;

            ModelState.Remove(nameof(Book.BranchId));

            if (await LibraryCodeExistsAsync(input.LibraryCode, id))
            {
                ModelState.AddModelError(
                    nameof(Book.LibraryCode),
                    "This library code is already being used.");
            }

            if (!Enum.IsDefined(typeof(ItemStatus), input.Status))
            {
                ModelState.AddModelError(
                    nameof(Book.Status),
                    "Select a valid item status.");
            }

            var activeTransfer = await _context.ItemTransfers
                .AnyAsync(t =>
                    t.ItemId == id &&
                    t.Status == TransferStatus.InTransit);

            if (activeTransfer || book.Status == ItemStatus.InTransit)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This book is in transit. Receive the transfer " +
                    "at its destination before editing it.");
            }
            else if (input.Status == ItemStatus.InTransit)
            {
                ModelState.AddModelError(
                    nameof(Book.Status),
                    "Use the transfer workflow to dispatch a book.");
            }

            var activeLoan = await _context.Loans
                .AnyAsync(loan =>
                    loan.ItemId == id &&
                    loan.ReturnedDate == null);

            if ((book.Status == ItemStatus.Borrowed || activeLoan) &&
                input.Status != ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Book.Status),
                    "A borrowed book must be returned through Reception " +
                    "before its status can change.");
            }

            if (book.Status != ItemStatus.Borrowed &&
                input.Status == ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Book.Status),
                    "Use the Reception borrowing process " +
                    "to mark a book as borrowed.");
            }

            var allocatedHold = await _context.Holds.AnyAsync(hold =>
                hold.ItemId == id && (hold.Status == HoldStatus.Allocated ||
                    hold.Status == HoldStatus.ReadyForPickup));
            if (allocatedHold && input.Status != ItemStatus.Available)
            {
                ModelState.AddModelError(nameof(Book.Status),
                    "Cancel the pickup allocation before withdrawing or changing the condition of this item.");
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            book.Author = input.Author;
            book.BookGenre = input.BookGenre;
            book.Isbn = input.Isbn;
            book.PublicationYear = input.PublicationYear;
            book.Publisher = input.Publisher;
            book.LibraryCode = input.LibraryCode;
            book.Name = input.Name;
            book.Description = input.Description;
            book.Status = input.Status;

            // Deliberately preserve book.BranchId.

            try
            {
                await _context.SaveChangesAsync();
                var allocation = await _holds.RefreshLockedAsync(book);
                await transaction.CommitAsync();
                await _holds.NotifyReadyAfterCommitAsync(allocation);

                TempData["SuccessMessage"] =
                    $"Book '{book.Name}' was updated. " +
                    "Its branch location was preserved.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.DisposeAsync();
                _context.ChangeTracker.Clear();
                if (!await BookExistsAsync(id))
                {
                    return NotFound();
                }

                ModelState.AddModelError(
                    string.Empty,
                    "The book was changed by another user. " +
                    "Reload the page and try again.");
            }
            catch (DbUpdateException)
            {
                await transaction.DisposeAsync();
                _context.ChangeTracker.Clear();
                ModelState.AddModelError(
                    string.Empty,
                    "The book could not be updated. " +
                    "Check that its library code is unique.");
            }

            catch (SqlException exception) when (exception.Number is 1205 or 1222 or 51001)
            {
                await transaction.DisposeAsync();
                _context.ChangeTracker.Clear();
                ModelState.AddModelError(string.Empty,
                    "This item changed during the update. Reload the page before trying again.");
            }

            return View(input);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .AsNoTracking()
                .Include(book => book.Branch)
                .FirstOrDefaultAsync(book => book.Id == id);

            return book is null ? NotFound() : View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            var hasLoanHistory = await _context.Loans
                .AnyAsync(loan => loan.ItemId == id);

            var hasTransferHistory = await _context.ItemTransfers
                .AnyAsync(transfer => transfer.ItemId == id);

            if (book.Status == ItemStatus.Borrowed ||
                book.Status == ItemStatus.InTransit ||
                hasLoanHistory ||
                hasTransferHistory)
            {
                TempData["ErrorMessage"] =
                    "This book cannot be deleted because it is borrowed, " +
                    "in transit, or has loan or transfer history. " +
                    "Keep the record for audit purposes.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Book '{book.Name}' was deleted successfully.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] =
                    "This book could not be deleted. " +
                    "It may be referenced by another record. " +
                    "Keep the record and ask an administrator to review it.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<ReceptionDesk?> GetWorkingDeskAsync()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) ||
                HttpContext.Session.GetString("StaffLocationUserId")
                    != userId)
            {
                return null;
            }

            var branchId =
                HttpContext.Session.GetInt32("StaffBranchId");

            var deskId =
                HttpContext.Session.GetInt32("StaffDeskId");

            return await _context.ReceptionDesks
                .AsNoTracking()
                .Include(d => d.Branch)
                .SingleOrDefaultAsync(d =>
                    d.Id == deskId &&
                    d.BranchId == branchId &&
                    d.IsActive &&
                    d.Branch != null &&
                    d.Branch.IsActive);
        }

        private IActionResult RequestWorkingLocation()
        {
            TempData["LocationMessage"] =
                "Select an active branch and reception desk, " +
                "then return to Manage books to create the book.";

            return RedirectToAction("Index", "StaffLocation");
        }

        private async Task<bool> LibraryCodeExistsAsync(
            string libraryCode,
            int? excludedId = null)
        {
            var query = _context.Items
                .Where(item => item.LibraryCode == libraryCode);

            if (excludedId.HasValue)
            {
                query = query.Where(item => item.Id != excludedId.Value);
            }

            return await query.AnyAsync();
        }

        private async Task<bool> BookExistsAsync(int id)
        {
            return await _context.Books
                .AnyAsync(book => book.Id == id);
        }
    }
}