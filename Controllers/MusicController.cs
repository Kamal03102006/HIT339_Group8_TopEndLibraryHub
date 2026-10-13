using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MusicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusicController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? searchTerm,
            ItemStatus? status)
        {
            var musicItems = _context.MusicItems
                .AsNoTracking()
                .Include(music => music.Branch)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                musicItems = musicItems.Where(music =>
                    music.Name.Contains(searchTerm) ||
                    music.LibraryCode.Contains(searchTerm) ||
                    music.Artist.Contains(searchTerm) ||
                    music.MusicGenre.Contains(searchTerm) ||
                    music.Format.Contains(searchTerm));
            }

            if (status.HasValue)
            {
                musicItems = musicItems.Where(
                    music => music.Status == status);
            }

            ViewData["CurrentSearch"] = searchTerm;
            ViewData["CurrentStatus"] = status;

            return View(await musicItems
                .OrderBy(music => music.Name)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var music = await _context.MusicItems
                .AsNoTracking()
                .Include(music => music.Branch)
                .FirstOrDefaultAsync(music => music.Id == id);

            return music is null ? NotFound() : View(music);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var desk = await GetWorkingDeskAsync();

            if (desk is null)
            {
                return RequestWorkingLocation();
            }

            return View(new Music
            {
                BranchId = desk.BranchId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Artist,MusicGenre,ReleaseYear,Format," +
                "LibraryCode,Name,Description")]
            Music music)
        {
            var desk = await GetWorkingDeskAsync();

            if (desk is null)
            {
                return RequestWorkingLocation();
            }

            // Use the validated working branch, not browser input.
            music.BranchId = desk.BranchId;
            ModelState.Remove(nameof(Music.BranchId));

            if (await LibraryCodeExistsAsync(music.LibraryCode))
            {
                ModelState.AddModelError(
                    nameof(Music.LibraryCode),
                    "This library code is already being used.");
            }

            if (!ModelState.IsValid)
            {
                return View(music);
            }

            music.Status = ItemStatus.Available;
            music.DateAdded = DateTime.UtcNow;

            try
            {
                _context.MusicItems.Add(music);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Music item '{music.Name}' was created at " +
                    $"{desk.Branch!.Name}.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The music item could not be saved. Check that " +
                    "its library code is unique and its branch still exists.");

                return View(music);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var music = await _context.MusicItems
                .AsNoTracking()
                .Include(music => music.Branch)
                .FirstOrDefaultAsync(music => music.Id == id);

            return music is null ? NotFound() : View(music);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind(
                "Artist,MusicGenre,ReleaseYear,Format," +
                "LibraryCode,Name,Description,Status")]
            Music input)
        {
            var music = await _context.MusicItems
                .Include(music => music.Branch)
                .FirstOrDefaultAsync(music => music.Id == id);

            if (music is null)
            {
                return NotFound();
            }

            // Preserve the stored location during ordinary edits.
            input.BranchId = music.BranchId;
            input.Branch = music.Branch;
            input.Id = music.Id;
            input.DateAdded = music.DateAdded;

            ModelState.Remove(nameof(Music.BranchId));

            if (await LibraryCodeExistsAsync(input.LibraryCode, id))
            {
                ModelState.AddModelError(
                    nameof(Music.LibraryCode),
                    "This library code is already being used.");
            }

            if (!Enum.IsDefined(typeof(ItemStatus), input.Status))
            {
                ModelState.AddModelError(
                    nameof(Music.Status),
                    "Select a valid item status.");
            }

            var activeTransfer = await _context.ItemTransfers
                .AnyAsync(t =>
                    t.ItemId == id &&
                    t.Status == TransferStatus.InTransit);

            if (activeTransfer ||
                music.Status == ItemStatus.InTransit)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This item is in transit. Receive the transfer " +
                    "at its destination before editing it.");
            }
            else if (input.Status == ItemStatus.InTransit)
            {
                ModelState.AddModelError(
                    nameof(Music.Status),
                    "Use the transfer workflow to dispatch an item.");
            }

            var activeLoan = await _context.Loans.AnyAsync(loan =>
                loan.ItemId == id &&
                loan.ReturnedDate == null);

            if ((music.Status == ItemStatus.Borrowed || activeLoan) &&
                input.Status != ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Music.Status),
                    "A borrowed music item must be returned through " +
                    "Reception before its status can change.");
            }

            if (music.Status != ItemStatus.Borrowed &&
                input.Status == ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Music.Status),
                    "Use the Reception borrowing process " +
                    "to mark an item as borrowed.");
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            music.Artist = input.Artist;
            music.MusicGenre = input.MusicGenre;
            music.ReleaseYear = input.ReleaseYear;
            music.Format = input.Format;
            music.LibraryCode = input.LibraryCode;
            music.Name = input.Name;
            music.Description = input.Description;
            music.Status = input.Status;

            // Do not change music.BranchId here.

            try
            {
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Music item '{music.Name}' was updated. " +
                    "Its branch location was preserved.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MusicExistsAsync(id))
                {
                    return NotFound();
                }

                ModelState.AddModelError(
                    string.Empty,
                    "The item was changed by another user. " +
                    "Reload the page and try again.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The item could not be updated. " +
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

            var music = await _context.MusicItems
                .AsNoTracking()
                .Include(music => music.Branch)
                .FirstOrDefaultAsync(music => music.Id == id);

            return music is null ? NotFound() : View(music);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var music = await _context.MusicItems.FindAsync(id);

            if (music is null)
            {
                return NotFound();
            }

            var hasLoanHistory = await _context.Loans
                .AnyAsync(loan => loan.ItemId == id);

            var hasTransferHistory = await _context.ItemTransfers
                .AnyAsync(transfer => transfer.ItemId == id);

            if (music.Status == ItemStatus.Borrowed ||
                music.Status == ItemStatus.InTransit ||
                hasLoanHistory ||
                hasTransferHistory)
            {
                TempData["ErrorMessage"] =
                    "This music item cannot be deleted because it is " +
                    "borrowed, in transit, or has loan or transfer history. " +
                    "Keep the record for audit purposes.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.MusicItems.Remove(music);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Music item '{music.Name}' was deleted successfully.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] =
                    "This item could not be deleted. It may be referenced " +
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
                "then return to Manage music to create the item.";

            return RedirectToAction("Index", "StaffLocation");
        }

        private Task<bool> LibraryCodeExistsAsync(
            string libraryCode,
            int? excludedId = null)
        {
            return _context.Items.AnyAsync(item =>
                item.LibraryCode == libraryCode &&
                (!excludedId.HasValue ||
                 item.Id != excludedId.Value));
        }

        private Task<bool> MusicExistsAsync(int id)
        {
            return _context.MusicItems.AnyAsync(
                music => music.Id == id);
        }
    }
}