using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.ViewModels;
using TopEndLibraryHub.Services;

namespace TopEndLibraryHub.Controllers
{
    [ResponseCache(
        NoStore = true,
        Location = ResponseCacheLocation.None)]
    public class KioskController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _users;
        private readonly SignInManager<IdentityUser> _signIn;
        private readonly KioskCheckoutService _checkout;

        public KioskController(
            ApplicationDbContext db,
            UserManager<IdentityUser> users,
            SignInManager<IdentityUser> signIn,
            KioskCheckoutService checkout)
        {
            _db = db;
            _users = users;
            _signIn = signIn;
            _checkout = checkout;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? status,
            string? type)
        {
            var query = _db.Items
                .AsNoTracking()
                .AsQueryable();

            search = search?.Trim();

            if (!string.IsNullOrEmpty(search))
            {
                if (search.Length > 100)
                    search = search[..100];

                query = query.Where(i =>
                    i.Name.Contains(search) ||
                    i.LibraryCode.Contains(search) ||
                    i.Description.Contains(search));
            }

            if (Enum.TryParse<ItemStatus>(
                status,
                true,
                out var parsed) &&
                Enum.IsDefined(parsed))
            {
                query = query.Where(
                    i => i.Status == parsed);
            }

            if (type == "Book")
                query = query.OfType<Book>();
            else if (type == "Toy")
                query = query.OfType<Toy>();
            else if (type == "Music")
                query = query.OfType<Music>();

            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Type = type;

            var items = await query
                .OrderBy(i => i.Name)
                .ThenBy(i => i.Id)
                .Take(100)
                .ToListAsync();

            return View(items);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Scan()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Scan(
            string? libraryCode)
        {
            var code = (libraryCode ?? "")
                .Trim()
                .ToUpperInvariant();

            if (!Regex.IsMatch(
                code,
                "^[A-Z]{2,4}-[0-9]{4}$"))
            {
                ViewBag.Message =
                    "Enter a valid library code, such as BK-0001.";

                return View();
            }

            var item = await _db.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    i => i.LibraryCode == code);

            if (item == null)
            {
                ViewBag.Message =
                    "No item was found for that code.";
            }

            return View(item);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Account()
        {
            var vm = new KioskAccountViewModel();

            var userId = _users.GetUserId(User);

            if (userId == null)
                return Challenge();

            var borrower = await _db.Borrowers
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    b => b.IdentityUserId == userId);

            if (borrower == null)
            {
                vm.Message =
                    "Your account is not linked to a library membership. Please contact staff.";

                return View(vm);
            }

            vm.FullName = borrower.FullName;
            vm.IsActive = borrower.IsActive;

            if (!borrower.IsActive)
            {
                vm.Message =
                    "Your library membership is inactive.";
            }

            vm.Loans = await _db.Loans
                .AsNoTracking()
                .Where(l =>
                    l.BorrowerId == borrower.Id &&
                    l.ReturnedDate == null)
                .Select(l => new KioskLoanRow
                {
                    Name = l.Item.Name,
                    LibraryCode = l.Item.LibraryCode,
                    DueDate = l.DueDate,
                    FineAmount = l.FineAmount
                })
                .ToListAsync();

            vm.OutstandingFines = await _db.Loans
                .AsNoTracking()
                .Where(l =>
                    l.BorrowerId == borrower.Id &&
                    !l.FinePaid)
                .SumAsync(l => l.FineAmount);

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(
            string? libraryCode)
        {
            var userId = _users.GetUserId(User);

            if (userId == null)
                return Challenge();

            var code = (libraryCode ?? "")
                .Trim()
                .ToUpperInvariant();

            if (!Regex.IsMatch(
                code,
                "^[A-Z]{2,4}-[0-9]{4}$"))
            {
                TempData["KioskError"] =
                    "Please enter a valid library code.";

                return RedirectToAction(nameof(Scan));
            }

            // Call the actual checkout service.
            var result = await _checkout.CheckoutAsync(
                userId,
                code);

            if (!result.Success)
            {
                TempData["KioskError"] =
                    result.Message;

                return RedirectToAction(nameof(Scan));
            }

            // Show the confirmation only after success.
            TempData["KioskSuccess"] =
                $"{result.ItemName} checked out successfully. " +
                $"Due date: {result.DueDate:dd MMM yyyy}.";

            return RedirectToAction(
                nameof(CheckoutSuccess));
        }

        [HttpGet]
        [Authorize]
        public IActionResult CheckoutSuccess()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish()
        {
            await _signIn.SignOutAsync();

            Response.Headers.CacheControl =
                "no-store, no-cache, must-revalidate";

            Response.Headers.Pragma = "no-cache";

            return RedirectToAction(nameof(Index));
        }
    }
}