using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Controllers
{
    [ApiController]
    [Route("api/catalogue")]
    [ResponseCache(
        NoStore = true,
        Location = ResponseCacheLocation.None)]
    public class CatalogueApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CatalogueApiController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /api/catalogue
        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string? search,
            [FromQuery] int limit = 30)
        {
            if (search?.Length > 100)
            {
                return BadRequest(new
                {
                    Error = "Search cannot exceed 100 characters."
                });
            }

            if (limit < 1 || limit > 100)
            {
                return BadRequest(new
                {
                    Error = "Limit must be between 1 and 100."
                });
            }

            var query = _db.Items.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();

                query = query.Where(i =>
                    i.Name.Contains(term) ||
                    i.LibraryCode.Contains(term) ||
                    i.Description.Contains(term));
            }

            var items = await query
                .OrderBy(i => i.Name)
                .ThenBy(i => i.Id)
                .Take(limit)
                .Select(i => new
                {
                    i.LibraryCode,
                    i.Name,
                    i.Description,
                    Status = i.Status.ToString(),
                    Available =
                        i.Status == ItemStatus.Available &&
                        !_db.Loans.Any(l =>
                            l.ItemId == i.Id &&
                            l.ReturnedDate == null) &&
                        !_db.ItemTransfers.Any(t =>
                            t.ItemId == i.Id &&
                            t.Status == TransferStatus.InTransit) &&
                        !_db.Holds.Any(h =>
                            h.ItemId == i.Id &&
                            (
                                h.Status == HoldStatus.Waiting ||
                                h.Status == HoldStatus.Allocated ||
                                h.Status == HoldStatus.ReadyForPickup
                            )) &&
                        _db.Branches.Any(b =>
                            b.Id == i.BranchId &&
                            b.IsActive)
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/catalogue/available
        [HttpGet("available")]
        public async Task<IActionResult> Available(
            [FromQuery] int limit = 30)
        {
            if (limit < 1 || limit > 100)
            {
                return BadRequest(new
                {
                    Error = "Limit must be between 1 and 100."
                });
            }

            var items = await _db.Items
                .AsNoTracking()
                .Where(i =>
                    i.Status == ItemStatus.Available &&
                    !_db.Loans.Any(l =>
                        l.ItemId == i.Id &&
                        l.ReturnedDate == null) &&
                    !_db.ItemTransfers.Any(t =>
                        t.ItemId == i.Id &&
                        t.Status == TransferStatus.InTransit) &&
                    !_db.Holds.Any(h =>
                        h.ItemId == i.Id &&
                        (
                            h.Status == HoldStatus.Waiting ||
                            h.Status == HoldStatus.Allocated ||
                            h.Status == HoldStatus.ReadyForPickup
                        )) &&
                    _db.Branches.Any(b =>
                        b.Id == i.BranchId &&
                        b.IsActive))
                .OrderBy(i => i.Name)
                .ThenBy(i => i.Id)
                .Take(limit)
                .Select(i => new
                {
                    i.LibraryCode,
                    i.Name,
                    i.Description,
                    Status = "Available",
                    Available = true
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/catalogue/categories
        [HttpGet("categories")]
        public IActionResult Categories()
        {
            return Ok(new[]
            {
                new { Name = "Book" },
                new { Name = "Music" },
                new { Name = "Toy" }
            });
        }

        // GET /api/catalogue/status
        [HttpGet("status")]
        public async Task<IActionResult> Status()
        {
            try
            {
                bool connected =
                    await _db.Database.CanConnectAsync();

                if (!connected)
                {
                    return StatusCode(503, new
                    {
                        Service = "TopEndLibraryHub Catalogue API",
                        Status = "Unavailable",
                        DatabaseConnected = false
                    });
                }

                var branches = await _db.Branches
                    .AsNoTracking()
                    .OrderBy(b => b.Name)
                    .Select(b => new
                    {
                        b.BranchCode,
                        b.Name,
                        b.IsActive
                    })
                    .ToListAsync();

                return Ok(new
                {
                    Service = "TopEndLibraryHub Catalogue API",
                    Status = "Operational",
                    DatabaseConnected = true,
                    Branches = branches,
                    BranchOperatingHours =
                        "Not configured in the current Branch model.",
                    CheckedAtUtc = DateTime.UtcNow
                });
            }
            catch
            {
                return StatusCode(503, new
                {
                    Service = "TopEndLibraryHub Catalogue API",
                    Status = "Unavailable",
                    DatabaseConnected = false
                });
            }
        }

        // GET /api/catalogue/BK-0001
        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(
            string code)
        {
            if (string.IsNullOrWhiteSpace(code) ||
                code.Length > 20)
            {
                return BadRequest(new
                {
                    Error = "Invalid library code."
                });
            }

            code = code.Trim().ToUpperInvariant();

            var item = await _db.Items
                .AsNoTracking()
                .Where(i => i.LibraryCode == code)
                .Select(i => new
                {
                    i.LibraryCode,
                    i.Name,
                    i.Description,
                    Status = i.Status.ToString(),
                    Available =
                        i.Status == ItemStatus.Available &&
                        !_db.Loans.Any(l =>
                            l.ItemId == i.Id &&
                            l.ReturnedDate == null) &&
                        !_db.ItemTransfers.Any(t =>
                            t.ItemId == i.Id &&
                            t.Status == TransferStatus.InTransit) &&
                        !_db.Holds.Any(h =>
                            h.ItemId == i.Id &&
                            (
                                h.Status == HoldStatus.Waiting ||
                                h.Status == HoldStatus.Allocated ||
                                h.Status == HoldStatus.ReadyForPickup
                            )) &&
                        _db.Branches.Any(b =>
                            b.Id == i.BranchId &&
                            b.IsActive)
                })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound(new
                {
                    Error = "Item not found."
                });
            }

            return Ok(item);
        }
    }
}