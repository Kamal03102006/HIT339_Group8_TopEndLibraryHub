using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ToysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToysController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? searchTerm,
            ItemStatus? status)
        {
            var toys = _context.Toys
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                toys = toys.Where(toy =>
                    toy.Name.Contains(searchTerm) ||
                    toy.LibraryCode.Contains(searchTerm) ||
                    toy.ToyType.Contains(searchTerm) ||
                    toy.RecommendedAge.Contains(searchTerm) ||
                    (toy.Manufacturer != null &&
                     toy.Manufacturer.Contains(searchTerm)) ||
                    (toy.Material != null &&
                     toy.Material.Contains(searchTerm)));
            }

            if (status.HasValue)
            {
                toys = toys.Where(toy => toy.Status == status);
            }

            ViewData["CurrentSearch"] = searchTerm;
            ViewData["CurrentStatus"] = status;

            return View(await toys
                .OrderBy(toy => toy.Name)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var toy = await _context.Toys
                .AsNoTracking()
                .FirstOrDefaultAsync(toy => toy.Id == id);

            return toy is null ? NotFound() : View(toy);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "ToyType,RecommendedAge,Manufacturer,Material," +
                "LibraryCode,Name,Description")]
            Toy toy)
        {
            if (await LibraryCodeExistsAsync(toy.LibraryCode))
            {
                ModelState.AddModelError(
                    nameof(Toy.LibraryCode),
                    "This library code is already being used.");
            }

            if (!ModelState.IsValid)
            {
                return View(toy);
            }

            toy.Status = ItemStatus.Available;
            toy.DateAdded = DateTime.UtcNow;

            try
            {
                _context.Toys.Add(toy);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Toy '{toy.Name}' was created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    nameof(Toy.LibraryCode),
                    "The toy could not be saved. Check that the library code is unique.");

                return View(toy);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var toy = await _context.Toys.FindAsync(id);

            return toy is null ? NotFound() : View(toy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind(
                "ToyType,RecommendedAge,Manufacturer,Material," +
                "LibraryCode,Name,Description,Status")]
            Toy input)
        {
            var toy = await _context.Toys.FindAsync(id);

            if (toy is null)
            {
                return NotFound();
            }

            if (await LibraryCodeExistsAsync(input.LibraryCode, id))
            {
                ModelState.AddModelError(
                    nameof(Toy.LibraryCode),
                    "This library code is already being used.");
            }

            if (toy.Status == ItemStatus.Borrowed &&
                input.Status != ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Toy.Status),
                    "A borrowed toy must be returned through Reception before its status can change.");
            }

            if (toy.Status != ItemStatus.Borrowed &&
                input.Status == ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Toy.Status),
                    "Use the Reception borrowing process to mark a toy as borrowed.");
            }

            if (!ModelState.IsValid)
            {
                input.Id = toy.Id;
                input.DateAdded = toy.DateAdded;
                return View(input);
            }

            toy.ToyType = input.ToyType;
            toy.RecommendedAge = input.RecommendedAge;
            toy.Manufacturer = input.Manufacturer;
            toy.Material = input.Material;
            toy.LibraryCode = input.LibraryCode;
            toy.Name = input.Name;
            toy.Description = input.Description;
            toy.Status = input.Status;

            try
            {
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Toy '{toy.Name}' was updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ToyExistsAsync(id))
                {
                    return NotFound();
                }

                ModelState.AddModelError(
                    string.Empty,
                    "The toy was changed by another user. Reload the page and try again.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    nameof(Toy.LibraryCode),
                    "The toy could not be updated. Check that the library code is unique.");
            }

            input.Id = toy.Id;
            input.DateAdded = toy.DateAdded;
            return View(input);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var toy = await _context.Toys
                .AsNoTracking()
                .FirstOrDefaultAsync(toy => toy.Id == id);

            return toy is null ? NotFound() : View(toy);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toy = await _context.Toys.FindAsync(id);

            if (toy is null)
            {
                return NotFound();
            }

            var hasLoanHistory = await _context.Loans
                .AnyAsync(loan => loan.ItemId == id);

            if (toy.Status == ItemStatus.Borrowed || hasLoanHistory)
            {
                TempData["ErrorMessage"] =
                    "This toy cannot be deleted because it is borrowed or has loan history. " +
                    "Change its status to Damaged or Destroyed instead.";

                return RedirectToAction(nameof(Index));
            }

            _context.Toys.Remove(toy);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Toy '{toy.Name}' was deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private Task<bool> ToyExistsAsync(int id)
        {
            return _context.Toys.AnyAsync(toy => toy.Id == id);
        }

        private Task<bool> LibraryCodeExistsAsync(
            string libraryCode,
            int? excludedId = null)
        {
            var normalisedCode = libraryCode.Trim();

            return _context.Items.AnyAsync(item =>
                item.LibraryCode == normalisedCode &&
                (!excludedId.HasValue || item.Id != excludedId.Value));
        }
    }
}