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

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(
            string? searchTerm,
            ItemStatus? status)
        {
            var books = _context.Books
                .AsNoTracking()
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

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(book => book.Id == id);

            if (book is null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Author,BookGenre,Isbn,PublicationYear,Publisher," +
                "LibraryCode,Name,Description")]
            Book book)
        {
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
                    $"Book '{book.Name}' was created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    nameof(Book.LibraryCode),
                    "The book could not be saved. Check that the library code is unique.");

                return View(book);
            }
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind(
                "Author,BookGenre,Isbn,PublicationYear,Publisher," +
                "LibraryCode,Name,Description,Status")]
            Book input)
        {
            var book = await _context.Books.FindAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            if (await LibraryCodeExistsAsync(input.LibraryCode, id))
            {
                ModelState.AddModelError(
                    nameof(Book.LibraryCode),
                    "This library code is already being used.");
            }

            if (book.Status == ItemStatus.Borrowed &&
                input.Status != ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Book.Status),
                    "A borrowed book must be returned through Reception before its status can change.");
            }

            if (book.Status != ItemStatus.Borrowed &&
                input.Status == ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Book.Status),
                    "Use the Reception borrowing process to mark a book as borrowed.");
            }

            if (!ModelState.IsValid)
            {
                input.Id = book.Id;
                input.DateAdded = book.DateAdded;
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

            try
            {
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Book '{book.Name}' was updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExistsAsync(id))
                {
                    return NotFound();
                }

                ModelState.AddModelError(
                    string.Empty,
                    "The book was changed by another user. Reload the page and try again.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    nameof(Book.LibraryCode),
                    "The book could not be updated. Check that the library code is unique.");
            }

            input.Id = book.Id;
            input.DateAdded = book.DateAdded;
            return View(input);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(book => book.Id == id);

            if (book is null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
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

            if (book.Status == ItemStatus.Borrowed || hasLoanHistory)
            {
                TempData["ErrorMessage"] =
                    "This book cannot be deleted because it is borrowed or has loan history. " +
                    "Change its status to Damaged or Destroyed instead.";

                return RedirectToAction(nameof(Index));
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Book '{book.Name}' was deleted successfully.";

            return RedirectToAction(nameof(Index));
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