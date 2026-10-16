using System.Data;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers;

[Authorize(Roles = "Admin")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class CsvImportController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly LibraryClock _clock;
    private readonly ILogger<CsvImportController> _logger;

    public CsvImportController(ApplicationDbContext db, LibraryClock clock,
        ILogger<CsvImportController> logger)
    { _db = db; _clock = clock; _logger = logger; }

    [HttpGet]
    public async Task<IActionResult> Index() => View(await FormAsync());

    [HttpPost, ValidateAntiForgeryToken]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> Upload(IFormFile? file, int branchId,
        CancellationToken cancellationToken = default)
    {
        var result = await FormAsync(branchId);
        IActionResult Invalid(string message)
        { result.Message = message; return View("Index", result); }
        if (file is null || file.Length == 0 ||
            !string.Equals(Path.GetExtension(file.FileName), ".csv", StringComparison.OrdinalIgnoreCase))
            return Invalid("Please choose a non-empty CSV file.");
        if (file.Length > 5 * 1024 * 1024) return Invalid("The CSV must be 5 MB or smaller.");
        if (!result.Branches.Any(branch => branch.Id == branchId))
            return Invalid("Please select an active library branch.");

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
            if (!await csv.ReadAsync()) return Invalid("The CSV is empty.");
            csv.ReadHeader();
            var headers = csv.HeaderRecord ?? Array.Empty<string>();
            int Column(string name) => Array.FindIndex(headers, header =>
                string.Equals(header.Trim().TrimStart('\uFEFF'), name, StringComparison.OrdinalIgnoreCase));
            var required = new[] { "Title", "Author", "ISBN", "Category", "Quantity", "PublicationYear" };
            var missing = required.Where(name => Column(name) < 0).ToArray();
            if (missing.Length > 0) return Invalid("Missing columns: " + string.Join(", ", missing) + ".");
            if (headers.Select(header => header.Trim().TrimStart('\uFEFF'))
                .Distinct(StringComparer.OrdinalIgnoreCase).Count() != headers.Length)
                return Invalid("Column headings must be unique.");

            await using var transaction = _db.Database.IsRelational()
                ? await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken)
                : null;
            var usedCodes = (await _db.Items.AsNoTracking().Select(item => item.LibraryCode)
                .ToListAsync(cancellationToken)).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var usedIsbns = (await _db.Books.AsNoTracking().Where(book => book.Isbn != null)
                .Select(book => book.Isbn!).ToListAsync(cancellationToken))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var books = new List<Book>();
            var nextCode = 1;
            var yearLimit = Math.Min(2100, _clock.Today.Year + 1);
            while (await csv.ReadAsync())
            {
                cancellationToken.ThrowIfCancellationRequested();
                string Read(string name) => csv.GetField(Column(name))?.Trim() ?? string.Empty;
                var title = Read("Title"); var author = Read("Author");
                var isbn = Read("ISBN"); var genre = Read("Category");
                var publisher = Column("Publisher") >= 0 ? Read("Publisher") : string.Empty;
                var errors = new List<string>();
                if (title.Length is < 2 or > 100) errors.Add("Title must contain 2-100 characters");
                if (author.Length is < 1 or > 100) errors.Add("Author is required and must be at most 100 characters");
                if (isbn.Length is < 1 or > 20) errors.Add("ISBN is required and must be at most 20 characters");
                if (genre.Length is < 1 or > 50) errors.Add("Category is required and must be at most 50 characters");
                if (publisher.Length > 100) errors.Add("Publisher must be at most 100 characters");
                if (!int.TryParse(Read("Quantity"), out var quantity) || quantity is < 1 or > 100)
                    errors.Add("Quantity must be a whole number from 1 to 100");
                if (!int.TryParse(Read("PublicationYear"), out var year) || year < 1000 || year > yearLimit)
                    errors.Add($"PublicationYear must be from 1000 to {yearLimit}");
                if (usedIsbns.Contains(isbn)) errors.Add("ISBN already exists in the catalogue or an earlier row");
                if (errors.Count > 0)
                {
                    result.SkippedRows++;
                    if (result.Rows.Count < 200) result.Rows.Add(new(csv.Parser.Row,
                        title, "Skipped", string.Join("; ", errors) + "."));
                    continue;
                }
                var firstCode = string.Empty;
                var lastCode = string.Empty;
                for (var copy = 0; copy < quantity; copy++)
                {
                    string code;
                    do
                    {
                        if (nextCode > 9999) throw new InvalidDataException("No unused BK-0001 to BK-9999 codes remain.");
                        code = $"BK-{nextCode++:D4}";
                    } while (usedCodes.Contains(code));
                    usedCodes.Add(code); firstCode = firstCode.Length == 0 ? code : firstCode; lastCode = code;
                    books.Add(new Book
                    {
                        BranchId = branchId, LibraryCode = code, Name = title, Author = author,
                        BookGenre = genre, Isbn = isbn, PublicationYear = year,
                        Publisher = publisher.Length == 0 ? null : publisher,
                        Description = $"Imported book: {title}", Status = ItemStatus.Available,
                        DateAdded = _clock.UtcNow
                    });
                }
                usedIsbns.Add(isbn);
                result.ImportedRows++;
                if (result.Rows.Count < 200) result.Rows.Add(new(csv.Parser.Row, title,
                    "Imported", $"{quantity} physical copies; codes {firstCode} to {lastCode}."));
            }
            _db.Books.AddRange(books);
            await _db.SaveChangesAsync(cancellationToken);
            if (transaction is not null) await transaction.CommitAsync(cancellationToken);
            result.ImportedCopies = books.Count;
            result.Succeeded = true;
            result.Message = $"{books.Count} physical copies imported from {result.ImportedRows} rows; {result.SkippedRows} rows skipped.";
            return View("Index", result);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
        catch (InvalidDataException exception)
        {
            _db.ChangeTracker.Clear(); result.Rows.Clear(); result.ImportedRows = 0;
            return Invalid(exception.Message + " No new copies were saved.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "CSV book import failed");
            _db.ChangeTracker.Clear(); result.Rows.Clear(); result.ImportedRows = 0;
            return Invalid("Import failed; no new copies were saved. Check the CSV structure or ask the administrator to review the server log.");
        }
    }

    private async Task<CsvImportViewModel> FormAsync(int branchId = 0) => new()
    {
        BranchId = branchId,
        Branches = await _db.Branches.AsNoTracking().Where(branch => branch.IsActive)
            .OrderBy(branch => branch.Name).ToListAsync()
    };
}
