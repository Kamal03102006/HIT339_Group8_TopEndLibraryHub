using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Controllers
{
    [ApiController]
    [Route("api/catalogue")]
    public class CatalogueApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CatalogueApiController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/catalogue
        // GET: api/catalogue?search=book
        // GET: api/catalogue?limit=20

        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string? search,
            [FromQuery] int limit = 30)
        {
            if (search != null && search.Length > 100)
            {
                return BadRequest(new
                {
                    error = "Search must be 100 characters or fewer."
                });
            }

            if (limit < 1 || limit > 100)
            {
                return BadRequest(new
                {
                    error = "Limit must be between 1 and 100."
                });
            }

            var query = _context.Items
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim();

                query = query.Where(i =>
                    i.Name.Contains(term) ||
                    i.LibraryCode.Contains(term) ||
                    i.Description.Contains(term));
            }

            var records = await query
                .OrderBy(i => i.Name)
                .ThenBy(i => i.Id)
                .Take(limit)
                .Select(i => new
                {
                    i.LibraryCode,
                    i.Name,
                    i.Description,
                    Status = i.Status,
                    Available = i.Status == ItemStatus.Available
                })
                .ToListAsync();

            Response.Headers.CacheControl = "no-store";

            return Ok(records);
        }

        // GET: api/catalogue/BK-0001

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(
            string code)
        {
            if (string.IsNullOrWhiteSpace(code)
                || code.Length > 20)
            {
                return BadRequest(new
                {
                    error = "Invalid library code."
                });
            }

            var item = await _context.Items
                .AsNoTracking()
                .Where(i => i.LibraryCode == code)
                .Select(i => new
                {
                    i.LibraryCode,
                    i.Name,
                    i.Description,
                    Status = i.Status,
                    Available = i.Status == ItemStatus.Available
                })
                .FirstOrDefaultAsync();

            Response.Headers.CacheControl = "no-store";

            if (item == null)
            {
                return NotFound(new
                {
                    error = "Item not found."
                });
            }

            return Ok(item);
        }
    }
}