using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers
{
    [Authorize(Roles = "Admin,Reception")]
    public class StaffLocationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffLocationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new StaffLocationViewModel();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Do not reuse another staff member's selected location.
            if (HttpContext.Session.GetString("StaffLocationUserId")
                == userId)
            {
                model.BranchId =
                    HttpContext.Session.GetInt32("StaffBranchId") ?? 0;

                model.ReceptionDeskId =
                    HttpContext.Session.GetInt32("StaffDeskId") ?? 0;

                var desk = await _context.ReceptionDesks
                    .AsNoTracking()
                    .Include(d => d.Branch)
                    .SingleOrDefaultAsync(d =>
                        d.Id == model.ReceptionDeskId &&
                        d.BranchId == model.BranchId &&
                        d.IsActive &&
                        d.Branch != null &&
                        d.Branch.IsActive);

                if (desk is not null)
                {
                    model.CurrentLocation =
                        $"{desk.Branch!.Name} / {desk.Name}";
                }
                else
                {
                    model.BranchId = 0;
                    model.ReceptionDeskId = 0;
                    HttpContext.Session.Remove("StaffBranchId");
                    HttpContext.Session.Remove("StaffDeskId");
                    HttpContext.Session.Remove("StaffLocationUserId");
                }
            }

            await PopulateOptionsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            StaffLocationViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var branchExists = await _context.Branches
                .AnyAsync(b =>
                    b.Id == model.BranchId && b.IsActive);

            if (!branchExists)
            {
                ModelState.AddModelError(
                    nameof(model.BranchId),
                    "Select an active branch.");
            }

            var deskMatches = await _context.ReceptionDesks
                .AnyAsync(d =>
                    d.Id == model.ReceptionDeskId &&
                    d.BranchId == model.BranchId &&
                    d.IsActive);

            if (!deskMatches)
            {
                ModelState.AddModelError(
                    nameof(model.ReceptionDeskId),
                    "Select an active desk belonging to your chosen branch.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateOptionsAsync(model);
                return View(model);
            }

            HttpContext.Session.SetInt32(
                "StaffBranchId", model.BranchId);

            HttpContext.Session.SetInt32(
                "StaffDeskId", model.ReceptionDeskId);

            HttpContext.Session.SetString(
                "StaffLocationUserId", userId);

            TempData["LocationMessage"] = "Working location saved.";

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateOptionsAsync(
            StaffLocationViewModel model)
        {
            model.Branches = await _context.Branches
                .AsNoTracking()
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToListAsync();

            model.Desks = await _context.ReceptionDesks
                .AsNoTracking()
                .Where(d =>
                    d.IsActive &&
                    d.Branch != null &&
                    d.Branch.IsActive)
                .OrderBy(d => d.Branch!.Name)
                .ThenBy(d => d.Name)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Branch!.Name + " / " + d.DeskCode
                })
                .ToListAsync();
        }
    }
}