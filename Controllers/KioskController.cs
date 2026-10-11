using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Controllers
{
    public class KioskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KioskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? status,
            string? type)
        {
            var items = _context.Items
                .AsNoTracking()
                .AsQueryable();

            // Search by item name, code or description.
            search = search?.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (search.Length > 100)
                {
                    search = search.Substring(0, 100);
                }

                items = items.Where(i =>
                    i.Name.Contains(search) ||
                    i.LibraryCode.Contains(search) ||
                    i.Description.Contains(search));
            }

            // Filter by availability.
            if (Enum.TryParse<ItemStatus>(
                status, true, out var selectedStatus)
                && Enum.IsDefined(typeof(ItemStatus), selectedStatus))
            {
                items = items.Where(i =>
                    i.Status == selectedStatus);
            }

            // Filter by item category.
            if (type == "Book")
            {
                items = items.OfType<Book>();
            }
            else if (type == "Toy")
            {
                items = items.OfType<Toy>();
            }
            else if (type == "Music")
            {
                items = items.OfType<Music>();
            }

            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Type = type;

            var results = await items
                .OrderBy(i => i.Name)
                .ThenBy(i => i.Id)
                .Take(100)
                .ToListAsync();

            return View(results);
        }
    }
}