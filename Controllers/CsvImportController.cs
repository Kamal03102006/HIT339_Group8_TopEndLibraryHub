using System.Data;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Controllers;

[Authorize(Roles = "Admin")]
public class CsvImportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CsvImportController> _logger;

    public CsvImportController(ApplicationDbContext context, ILogger<CsvImportController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.Branches = await _context.Branches.AsNoTracking()
            .Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile? file, int branchId)
    {
        if (file is null || file.Length == 0 ||
            !string.Equals(Path.GetExtension(file.FileName), ".csv", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "Please choose a non-empty CSV file.";
            return RedirectToAction(nameof(Index));
        }
        if (file.Length > 5 * 1024 * 1024)
        {
            TempData["ErrorMessage"] = "CSV file must be 5 MB or smaller.";
            return RedirectToAction(nameof(Index));
        }
        if (!await _context.Branches.AnyAsync(b => b.Id == branchId && b.IsActive))
        {
            TempData["ErrorMessage"] = "Please select an active library branch.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                HeaderValidated = null,
                MissingFieldFound = null
            });
            if (!await csv.ReadAsync())
            {
                TempData["ErrorMessage"] = "CSV file is empty.";
                return RedirectToAction(nameof(Index));
            }
            csv.ReadHeader();
            var headers = csv.HeaderRecord ?? Array.Empty<string>();
            int Column(string header) => Array.FindIndex(headers, h =>
                string.Equals(h.Trim().TrimStart('\uFEFF'), header, StringComparison.OrdinalIgnoreCase));
            var required = new[] { "Title", "Author", "ISBN", "Category", "Quantity", "PublicationYear" };
            if (required.Any(h => Column(h) < 0))
            {
                TempData["ErrorMessage"] = "CSV requires Title, Author, ISBN, Category, Quantity and PublicationYear columns.";
                return RedirectToAction(nameof(Index));
            }

            // Keep codes unique across all item types and imports. Use a transaction
            // to prevent a partially saved upload when a database constraint fails.
            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var existingCodes = (await _context.Items.AsNoTracking()
                .Select(i => i.LibraryCode).ToListAsync())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var existingIsbns = (await _context.Books.AsNoTracking()
                .Where(b => b.Isbn != null).Select(b => b.Isbn!).ToListAsync())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var books = new List<Book>();
            var skippedRows = 0;
            var importedRows = 0;
            var nextCode = 1;
            var currentYear = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow, DarwinTimeZone()).Year;

            while (await csv.ReadAsync())
            {
                string Read(string name) => csv.GetField(Column(name))?.Trim() ?? string.Empty;
                var title = Read("Title");
                var author = Read("Author");
                var isbn = Read("ISBN");
                var genre = Read("Category");
                var publisher = Column("Publisher") >= 0 ? Read("Publisher") : string.Empty;
                var quantityOk = int.TryParse(Read("Quantity"), out var quantity)
                                 && quantity >= 1 && quantity <= 100;
                var yearOk = int.TryParse(Read("PublicationYear"), out var year)
                             && year >= 1000 && year <= currentYear + 1;
                if (title.Length is < 2 or > 100 || author.Length is < 1 or > 100 ||
                    isbn.Length is < 1 or > 20 || genre.Length is < 1 or > 50 ||
                    publisher.Length > 100 || !quantityOk || !yearOk || existingIsbns.Contains(isbn))
                {
                    skippedRows++;
                    continue;
                }

                for (var copy = 0; copy < quantity; copy++)
                {
                    string code;
                    do
                    {
                        if (nextCode > 9999)
                            throw new InvalidOperationException("All BK-0001 to BK-9999 library codes are used.");
                        code = $"BK-{nextCode++:D4}";
                    } while (existingCodes.Contains(code));

                    existingCodes.Add(code);
                    books.Add(new Book
                    {
                        BranchId = branchId,
                        LibraryCode = code,
                        Name = title,
                        Author = author,
                        BookGenre = genre,
                        Isbn = isbn, // Shared ISBN across physical copies is intentional.
                        PublicationYear = year,
                        Publisher = string.IsNullOrWhiteSpace(publisher) ? null : publisher,
                        Description = $"Imported book: {title}",
                        Status = ItemStatus.Available,
                        DateAdded = DateTime.UtcNow
                    });
                }
                existingIsbns.Add(isbn);
                importedRows++;
            }

            if (books.Count > 0)
            {
                _context.Books.AddRange(books);
                await _context.SaveChangesAsync();
            }
            await transaction.CommitAsync();
            TempData["SuccessMessage"] = $"{books.Count} physical copies imported from {importedRows} CSV rows; {skippedRows} rows skipped.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CSV book import failed");
            TempData["ErrorMessage"] = "Import failed; no new copies were saved. See server logs.";
        }
        return RedirectToAction(nameof(Index));
    }

    private static TimeZoneInfo DarwinTimeZone()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("Australia/Darwin"); }
        catch (TimeZoneNotFoundException) { return TimeZoneInfo.FindSystemTimeZoneById("AUS Central Standard Time"); }
    }
}
