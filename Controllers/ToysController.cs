using System.Security.Claims;
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
                .Include(toy => toy.Branch)
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
                .Include(toy => toy.Branch)
                .FirstOrDefaultAsync(toy => toy.Id == id);

            return toy is null ? NotFound() : View(toy);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var desk = await GetWorkingDeskAsync();

            if (desk is null)
            {
                return RequestWorkingLocation();
            }

            return View(new Toy
            {
                BranchId = desk.BranchId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "ToyType,RecommendedAge,Manufacturer,Material," +
                "LibraryCode,Name,Description")]
            Toy toy)
        {
            var desk = await GetWorkingDeskAsync();

            if (desk is null)
            {
                return RequestWorkingLocation();
            }

            // Assign the validated working branch.
            toy.BranchId = desk.BranchId;
            ModelState.Remove(nameof(Toy.BranchId));

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
                    $"Toy '{toy.Name}' was created at {desk.Branch!.Name}.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The toy could not be saved. Check that its " +
                    "library code is unique and its branch still exists.");

                return View(toy);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var toy = await _context.Toys
                .AsNoTracking()
                .Include(toy => toy.Branch)
                .FirstOrDefaultAsync(toy => toy.Id == id);

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
            var toy = await _context.Toys
                .Include(toy => toy.Branch)
                .FirstOrDefaultAsync(toy => toy.Id == id);

            if (toy is null)
            {
                return NotFound();
            }

            // Ordinary edits preserve the existing location.
            input.BranchId = toy.BranchId;
            input.Branch = toy.Branch;
            input.Id = toy.Id;
            input.DateAdded = toy.DateAdded;

            ModelState.Remove(nameof(Toy.BranchId));

            if (await LibraryCodeExistsAsync(input.LibraryCode, id))
            {
                ModelState.AddModelError(
                    nameof(Toy.LibraryCode),
                    "This library code is already being used.");
            }

            if (!Enum.IsDefined(typeof(ItemStatus), input.Status))
            {
                ModelState.AddModelError(
                    nameof(Toy.Status),
                    "Select a valid item status.");
            }

            var activeTransfer = await _context.ItemTransfers
                .AnyAsync(t =>
                    t.ItemId == id &&
                    t.Status == TransferStatus.InTransit);

            if (activeTransfer || toy.Status == ItemStatus.InTransit)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This toy is in transit. Receive the transfer " +
                    "at its destination before editing it.");
            }
            else if (input.Status == ItemStatus.InTransit)
            {
                ModelState.AddModelError(
                    nameof(Toy.Status),
                    "Use the transfer workflow to dispatch a toy.");
            }

            var activeLoan = await _context.Loans.AnyAsync(loan =>
                loan.ItemId == id &&
                loan.ReturnedDate == null);

            if ((toy.Status == ItemStatus.Borrowed || activeLoan) &&
                input.Status != ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Toy.Status),
                    "A borrowed toy must be returned through Reception " +
                    "before its status can change.");
            }

            if (toy.Status != ItemStatus.Borrowed &&
                input.Status == ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Toy.Status),
                    "Use the Reception borrowing process " +
                    "to mark a toy as borrowed.");
            }

            if (!ModelState.IsValid)
            {
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

            // Deliberately leave toy.BranchId unchanged.

            try
            {
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Toy '{toy.Name}' was updated. " +
                    "Its branch location was preserved.";

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
                    "The toy was changed by another user. " +
                    "Reload the page and try again.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The toy could not be updated. " +
                    "Check that its library code is unique.");
            }

            return View(input);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var toy = await _context.Toys
                .AsNoTracking()
                .Include(toy => toy.Branch)
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

            var hasTransferHistory = await _context.ItemTransfers
                .AnyAsync(transfer => transfer.ItemId == id);

            if (toy.Status == ItemStatus.Borrowed ||
                toy.Status == ItemStatus.InTransit ||
                hasLoanHistory ||
                hasTransferHistory)
            {
                TempData["ErrorMessage"] =
                    "This toy cannot be deleted because it is borrowed, " +
                    "in transit, or has loan or transfer history. " +
                    "Keep the record for audit purposes.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Toys.Remove(toy);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Toy '{toy.Name}' was deleted successfully.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] =
                    "This toy could not be deleted. It may be referenced " +
                    "by another record. Keep it and ask an administrator " +
                    "to review it.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<ReceptionDesk?> GetWorkingDeskAsync()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) ||
                HttpContext.Session.GetString("StaffLocationUserId")
                    != userId)
            {
                return null;
            }

            var branchId =
                HttpContext.Session.GetInt32("StaffBranchId");

            var deskId =
                HttpContext.Session.GetInt32("StaffDeskId");

            return await _context.ReceptionDesks
                .AsNoTracking()
                .Include(d => d.Branch)
                .SingleOrDefaultAsync(d =>
                    d.Id == deskId &&
                    d.BranchId == branchId &&
                    d.IsActive &&
                    d.Branch != null &&
                    d.Branch.IsActive);
        }

        private IActionResult RequestWorkingLocation()
        {
            TempData["LocationMessage"] =
                "Select an active branch and reception desk, " +
                "then return to Manage toys to create the toy.";

            return RedirectToAction("Index", "StaffLocation");
        }

        private Task<bool> ToyExistsAsync(int id)
        {
            return _context.Toys.AnyAsync(toy => toy.Id == id);
        }

        private Task<bool> LibraryCodeExistsAsync(
            string libraryCode,
            int? excludedId = null)
        {
            var normalisedCode =
                (libraryCode ?? string.Empty).Trim();

            return _context.Items.AnyAsync(item =>
                item.LibraryCode == normalisedCode &&
                (!excludedId.HasValue ||
                 item.Id != excludedId.Value));
        }
    }
}