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
                .FirstOrDefaultAsync(music => music.Id == id);

            return music is null ? NotFound() : View(music);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Artist,MusicGenre,ReleaseYear,Format," +
                "LibraryCode,Name,Description")]
            Music music)
        {
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
                    $"Music item '{music.Name}' was created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    nameof(Music.LibraryCode),
                    "The music item could not be saved. Check that the library code is unique.");

                return View(music);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var music = await _context.MusicItems.FindAsync(id);

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
            var music = await _context.MusicItems.FindAsync(id);

            if (music is null)
            {
                return NotFound();
            }

            if (await LibraryCodeExistsAsync(input.LibraryCode, id))
            {
                ModelState.AddModelError(
                    nameof(Music.LibraryCode),
                    "This library code is already being used.");
            }

            if (music.Status == ItemStatus.Borrowed &&
                input.Status != ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Music.Status),
                    "A borrowed music item must be returned through Reception before its status can change.");
            }

            if (music.Status != ItemStatus.Borrowed &&
                input.Status == ItemStatus.Borrowed)
            {
                ModelState.AddModelError(
                    nameof(Music.Status),
                    "Use the Reception borrowing process to mark an item as borrowed.");
            }

            if (!ModelState.IsValid)
            {
                input.Id = music.Id;
                input.DateAdded = music.DateAdded;
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

            try
            {
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Music item '{music.Name}' was updated successfully.";

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
                    "The item was changed by another user. Reload the page and try again.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    nameof(Music.LibraryCode),
                    "The item could not be updated. Check that the library code is unique.");
            }

            input.Id = music.Id;
            input.DateAdded = music.DateAdded;
            return View(input);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var music = await _context.MusicItems
                .AsNoTracking()
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

            if (music.Status == ItemStatus.Borrowed || hasLoanHistory)
            {
                TempData["ErrorMessage"] =
                    "This music item cannot be deleted because it is borrowed or has loan history. " +
                    "Change its status to Damaged or Destroyed instead.";

                return RedirectToAction(nameof(Index));
            }

            _context.MusicItems.Remove(music);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Music item '{music.Name}' was deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private Task<bool> LibraryCodeExistsAsync(
            string libraryCode,
            int? excludedId = null)
        {
            return _context.Items.AnyAsync(item =>
                item.LibraryCode == libraryCode &&
                (!excludedId.HasValue || item.Id != excludedId.Value));
        }

        private Task<bool> MusicExistsAsync(int id)
        {
            return _context.MusicItems.AnyAsync(
                music => music.Id == id);
        }
    }
}