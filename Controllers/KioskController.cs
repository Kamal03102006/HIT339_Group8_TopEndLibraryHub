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

        // Catalogue home
        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? status,
            string? type)
        {
            var items = _context.Items
                .AsNoTracking()
                .AsQueryable();

            search = search?.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (search.Length > 100)
                    search = search.Substring(0, 100);

                items = items.Where(i =>
                    i.Name.Contains(search) ||
                    i.LibraryCode.Contains(search) ||
                    i.Description.Contains(search));
            }

            if (Enum.TryParse<ItemStatus>(
                status, true, out var selectedStatus)
                && Enum.IsDefined(typeof(ItemStatus), selectedStatus))
            {
                items = items.Where(i =>
                    i.Status == selectedStatus);
            }

            if (type == "Book")
                items = items.OfType<Book>();
            else if (type == "Toy")
                items = items.OfType<Toy>();
            else if (type == "Music")
                items = items.OfType<Music>();

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

        // Scanner-style item lookup
        [HttpGet]
        public IActionResult Scan()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Scan(string? libraryCode)
        {
            string code = (libraryCode ?? "")
                .Trim()
                .ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(code) ||
                code.Length > 20)
            {
                ViewBag.Message =
                    "Enter a valid library code.";
                return View();
            }

            var item = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i =>
                    i.LibraryCode == code);

            if (item == null)
            {
                ViewBag.Message =
                    "No library item was found with that code.";
                return View();
            }

            return View(item);
        }

        // Account access entry point.
        // Private account details remain unavailable until
        // authenticated Identity users are securely linked
        // to their Borrower records.
        [HttpGet]
        public IActionResult Account()
        {
            return View();
        }
    }
}