
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TopEndLibraryHub.Controllers;

[Authorize(Roles = "Admin")]
public class CsvImportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CsvImportController> _logger;

    public CsvImportController(
        ApplicationDbContext context,
        ILogger<CsvImportController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.Branches = await _context.Branches
            .AsNoTracking()
            .Where(b => b.IsActive)
            .OrderBy(b => b.Name)
            .ToListAsync();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        IFormFile? file, int branchId)
    {
        if (file == null || file.Length == 0 ||
            !Path.GetExtension(file.FileName)
                .Equals(".csv",
                    StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] =
                "Please select a valid CSV file.";
            return RedirectToAction(nameof(Index));
        }

        if (file.Length > 5 * 1024 * 1024)
        {
            TempData["ErrorMessage"] =
                "CSV file must be smaller than 5 MB.";
            return RedirectToAction(nameof(Index));
        }

        var branchExists = await _context.Branches
            .AnyAsync(b => b.Id == branchId && b.IsActive);

        if (!branchExists)
        {
            TempData["ErrorMessage"] =
                "Please select an active library branch.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            var config = new CsvConfiguration(
                CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var csv = new CsvReader(reader, config);

            if (!await csv.ReadAsync())
            {
                TempData["ErrorMessage"] = "CSV file is empty.";
                return RedirectToAction(nameof(Index));
            }

            csv.ReadHeader();

            var headers = csv.HeaderRecord
                ?? Array.Empty<string>();

            string[] required =
            {
                "Title", "Author", "ISBN",
                "Category", "Quantity"
            };

            if (required.Any(h => !headers.Any(
                x => string.Equals(x, h,
                    StringComparison.OrdinalIgnoreCase))))
            {
                TempData["ErrorMessage"] =
                    "CSV requires Title, Author, ISBN, " +
                    "Category and Quantity columns.";
                return RedirectToAction(nameof(Index));
            }

            int Column(string name) =>
                Array.FindIndex(headers, h =>
                    string.Equals(h, name,
                        StringComparison.OrdinalIgnoreCase));

            var existingCodes = (await _context.Items
                .AsNoTracking()
                .Select(i => i.LibraryCode)
                .ToListAsync())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var existingIsbns = (await _context.Books
                .AsNoTracking()
                .Where(b => b.Isbn != null)
                .Select(b => b.Isbn!)
                .ToListAsync())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var books = new List<Book>();
            int skipped = 0;
            int nextCode = 1;

            while (await csv.ReadAsync())
            {
                string title = csv.GetField(Column("Title"))?.Trim() ?? "";
                string author = csv.GetField(Column("Author"))?.Trim() ?? "";
                string isbn = csv.GetField(Column("ISBN"))?.Trim() ?? "";
                string genre = csv.GetField(Column("Category"))?.Trim() ?? "";
                string quantityText = csv.GetField(
                    Column("Quantity"))?.Trim() ?? "";

                bool validQuantity = int.TryParse(
                    quantityText, out int quantity)
                    && quantity > 0;

                if (title.Length < 2 ||
                    title.Length > 100 ||
                    string.IsNullOrWhiteSpace(author) ||
                    author.Length > 100 ||
                    string.IsNullOrWhiteSpace(isbn) ||
                    isbn.Length > 20 ||
                    string.IsNullOrWhiteSpace(genre) ||
                    genre.Length > 50 ||
                    !validQuantity ||
                    existingIsbns.Contains(isbn))
                {
                    skipped++;
                    continue;
                }

                string libraryCode;

                do
                {
                    if (nextCode > 9999)
                    {
                        throw new InvalidOperationException(
                            "No available BK library codes remain.");
                    }

                    libraryCode = $"BK-{nextCode:D4}";
                    nextCode++;
                }
                while (existingCodes.Contains(libraryCode));

                var book = new Book
                {
                    BranchId = branchId,
                    LibraryCode = libraryCode,
                    Name = title,
                    Author = author,
                    BookGenre = genre,
                    Isbn = isbn,
                    Description = $"Imported book: {title}",
                    Status = ItemStatus.Available,
                    DateAdded = DateTime.UtcNow
                };

                books.Add(book);
                existingCodes.Add(libraryCode);
                existingIsbns.Add(isbn);
            }

            if (books.Count > 0)
            {
                _context.Books.AddRange(books);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] =
                $"{books.Count} books imported successfully. " +
                $"{skipped} rows skipped.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CSV book import failed");

            TempData["ErrorMessage"] =
                "Import failed. Check the application terminal " +
                "for error details.";
        }

        return RedirectToAction(nameof(Index));
    }
}

