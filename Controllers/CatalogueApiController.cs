using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;

namespace TopEndLibraryHub.Controllers;

[ApiController]
[Route("api/catalogue")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class CatalogueApiController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public CatalogueApiController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? search,
        [FromQuery] int limit = 30, [FromQuery] string? category = null,
        [FromQuery] int? branchId = null)
    {
        var error = await ValidateAsync(limit, category, branchId);
        if (error is not null) return BadRequest(new { error });
        if (search?.Length > 100) return BadRequest(new { error = "Search must be 100 characters or fewer." });
        var query = Filter(_db.Items.AsNoTracking(), category, branchId);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(item => item.Name.Contains(term) ||
                item.LibraryCode.Contains(term) || item.Description.Contains(term));
        }
        var availableIds = CirculationRules.GenerallyAvailable(_db).Select(item => item.Id);
        return Ok(await query.OrderBy(item => item.Name).ThenBy(item => item.Id).Take(limit)
            .Select(item => new
            {
                item.LibraryCode, item.Name, item.Description,
                Category = item is Book ? "Book" : item is Music ? "Music" : "Toy",
                Status = item.Status.ToString(), Available = availableIds.Contains(item.Id),
                BranchId = item.BranchId, Branch = item.Branch == null ? null : item.Branch.Name
            }).ToListAsync());
    }

    [HttpGet("available")]
    public async Task<IActionResult> Available([FromQuery] int limit = 30,
        [FromQuery] string? category = null, [FromQuery] int? branchId = null)
    {
        var error = await ValidateAsync(limit, category, branchId);
        if (error is not null) return BadRequest(new { error });
        var query = Filter(CirculationRules.GenerallyAvailable(_db, _db.Items.AsNoTracking()), category, branchId);
        return Ok(await query.OrderBy(item => item.Name).ThenBy(item => item.Id).Take(limit)
            .Select(item => new
            {
                item.LibraryCode, item.Name, item.Description,
                Category = item is Book ? "Book" : item is Music ? "Music" : "Toy",
                Status = "Available", Available = true,
                BranchId = item.BranchId, Branch = item.Branch == null ? null : item.Branch.Name
            }).ToListAsync());
    }

    [HttpGet("categories")]
    public IActionResult Categories() => Ok(new[]
    { new { Name = "Book" }, new { Name = "Music" }, new { Name = "Toy" } });

    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        try
        {
            if (!await _db.Database.CanConnectAsync()) return Unavailable();
            var branches = await _db.Branches.AsNoTracking().OrderBy(branch => branch.Name)
                .Select(branch => new { branch.Id, branch.BranchCode, branch.Name, branch.IsActive }).ToListAsync();
            return Ok(new
            {
                Service = "TopEndLibraryHub Catalogue API", Status = "Operational",
                DatabaseConnected = true, Branches = branches,
                StatusMeaning = "Service health and configured branch availability; not live opening hours.",
                CheckedAtUtc = DateTime.UtcNow
            });
        }
        catch { return Unavailable(); }
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length > 20)
            return BadRequest(new { error = "Invalid library code." });
        code = code.Trim().ToUpperInvariant();
        var availableIds = CirculationRules.GenerallyAvailable(_db).Select(item => item.Id);
        var item = await _db.Items.AsNoTracking().Where(item => item.LibraryCode == code)
            .Select(item => new
            {
                item.LibraryCode, item.Name, item.Description,
                Category = item is Book ? "Book" : item is Music ? "Music" : "Toy",
                Status = item.Status.ToString(), Available = availableIds.Contains(item.Id),
                BranchId = item.BranchId, Branch = item.Branch == null ? null : item.Branch.Name
            }).SingleOrDefaultAsync();
        if (item is null) return NotFound(new { error = "Item not found." });
        return Ok(item);
    }

    private ObjectResult Unavailable() => StatusCode(503, new
    { Service = "TopEndLibraryHub Catalogue API", Status = "Unavailable", DatabaseConnected = false });

    private async Task<string?> ValidateAsync(int limit, string? category, int? branchId)
    {
        if (limit is < 1 or > 100) return "Limit must be between 1 and 100.";
        if (!string.IsNullOrWhiteSpace(category) &&
            !new[] { "book", "music", "toy" }.Contains(category.Trim().ToLowerInvariant()))
            return "Category must be Book, Music or Toy.";
        if (branchId.HasValue && (branchId <= 0 ||
            !await _db.Branches.AnyAsync(branch => branch.Id == branchId && branch.IsActive)))
            return "Choose an active branch ID.";
        return null;
    }

    private static IQueryable<Item> Filter(IQueryable<Item> query, string? category, int? branchId)
    {
        query = category?.Trim().ToLowerInvariant() switch
        {
            "book" => query.OfType<Book>(), "music" => query.OfType<Music>(),
            "toy" => query.OfType<Toy>(), _ => query
        };
        return branchId.HasValue ? query.Where(item => item.BranchId == branchId.Value) : query;
    }
}
