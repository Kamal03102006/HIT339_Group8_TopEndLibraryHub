using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;

namespace TopEndLibraryHub.ViewComponents
{
    public class StaffLocationViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public StaffLocationViewComponent(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = HttpContext.User;

            if (!user.Identity!.IsAuthenticated ||
                !(user.IsInRole("Admin") ||
                  user.IsInRole("Reception")))
            {
                return Content(string.Empty);
            }

            var userId =
                user.FindFirstValue(ClaimTypes.NameIdentifier);

            var session = HttpContext.Session;

            if (string.IsNullOrEmpty(userId) ||
                session.GetString("StaffLocationUserId") != userId)
            {
                return View("Default", string.Empty);
            }

            var branchId = session.GetInt32("StaffBranchId");
            var deskId = session.GetInt32("StaffDeskId");

            var desk = await _context.ReceptionDesks
                .AsNoTracking()
                .Include(d => d.Branch)
                .SingleOrDefaultAsync(d =>
                    d.Id == deskId &&
                    d.BranchId == branchId &&
                    d.IsActive &&
                    d.Branch != null &&
                    d.Branch.IsActive);

            if (desk is null)
            {
                session.Remove("StaffBranchId");
                session.Remove("StaffDeskId");
                session.Remove("StaffLocationUserId");

                return View("Default", string.Empty);
            }

            var location =
                $"{desk.Branch!.Name} / {desk.DeskCode}";

            return View("Default", location);
        }
    }
}