using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;

namespace TopEndLibraryHub.Controllers
{
    [ApiController]
    [Route("api/catalogue")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CatalogueApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CatalogueApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /api/catalogue
        // GET /api/catalogue?search=book&limit=20
        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string? search,
            [FromQuery] int limit = 30)
        {
            if (search != null && search.Length > 100)
                return BadRequest(new
                {
                    error = "Search must be 100 characters or fewer."
                });

            if (limit < 1 || limit > 100)
                return BadRequest(new
                {
                    error = "Limit must be between 1 and 100."
                });

            var query = _context.Items.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim();

                query = query.Where(i =>
                    i.Name.Contains(term) ||
                    i.LibraryCode.Contains(term) ||
                    i.Description.Contains(term));
            }

            var availableIds = CirculationRules.GenerallyAvailable(_context).Select(i => i.Id);
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
                    Available = availableIds.Contains(i.Id),
                    Branch = i.Branch != null ? i.Branch.Name : null
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
                return BadRequest(new
                {
                    error = "Limit must be between 1 and 100."
                });

            var items = await CirculationRules.GenerallyAvailable(_context, _context.Items.AsNoTracking())
                .OrderBy(i => i.Name)
                .ThenBy(i => i.Id)
                .Take(limit)
                .Select(i => new
                {
                    i.LibraryCode,
                    i.Name,
                    i.Description,
                    Status = "Available",
                    Available = true,
                    Branch = i.Branch != null ? i.Branch.Name : null
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
                    await _context.Database.CanConnectAsync();

                if (!connected)
                {
                    return StatusCode(503, new
                    {
                        Service = "TopEndLibraryHub Catalogue API",
                        Status = "Unavailable",
                        DatabaseConnected = false
                    });
                }

                return Ok(new
                {
                    Service = "TopEndLibraryHub Catalogue API",
                    Status = "Operational",
                    DatabaseConnected = true,
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
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code) ||
                code.Length > 20)
            {
                return BadRequest(new
                {
                    error = "Invalid library code."
                });
            }

            code = code.Trim().ToUpperInvariant();
            var availableIds = CirculationRules.GenerallyAvailable(_context).Select(i => i.Id);
            var item = await _context.Items
                .AsNoTracking()
                .Where(i => i.LibraryCode == code)
                .Select(i => new
                {
                    i.LibraryCode,
                    i.Name,
                    i.Description,
                    Status = i.Status.ToString(),
                    Available = availableIds.Contains(i.Id),
                    Branch = i.Branch != null ? i.Branch.Name : null
                })
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new
                {
                    error = "Item not found."
                });

            return Ok(item);
        }
    }
}