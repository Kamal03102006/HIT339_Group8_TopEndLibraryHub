using System.Data;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers;

[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class KioskController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _users;
    private readonly SignInManager<IdentityUser> _signIn;
    private readonly IConfiguration _configuration;
    public KioskController(ApplicationDbContext db, UserManager<IdentityUser> users,
        SignInManager<IdentityUser> signIn, IConfiguration configuration)
    { _db = db; _users = users; _signIn = signIn; _configuration = configuration; }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, string? status, string? type)
    {
        var query = _db.Items.AsNoTracking().AsQueryable();
        search = search?.Trim();
        if (!string.IsNullOrEmpty(search))
        {
            if (search.Length > 100) search = search[..100];
            query = query.Where(i => i.Name.Contains(search) ||
                i.LibraryCode.Contains(search) || i.Description.Contains(search));
        }
        if (Enum.TryParse<ItemStatus>(status, true, out var parsed) &&
            Enum.IsDefined(parsed)) query = query.Where(i => i.Status == parsed);
        if (type == "Book") query = query.OfType<Book>();
        else if (type == "Toy") query = query.OfType<Toy>();
        else if (type == "Music") query = query.OfType<Music>();
        ViewBag.Search = search; ViewBag.Status = status; ViewBag.Type = type;
        return View(await query.OrderBy(i => i.Name).ThenBy(i => i.Id).Take(100).ToListAsync());
    }

    [HttpGet, Authorize]
    public IActionResult Scan() => View();

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Scan(string? libraryCode)
    {
        var code = (libraryCode ?? "").Trim().ToUpperInvariant();
        if (!Regex.IsMatch(code, "^[A-Z]{2,4}-[0-9]{4}$"))
        { ViewBag.Message = "Enter a valid item code, such as BK-0001."; return View(); }
        var item = await _db.Items.AsNoTracking().FirstOrDefaultAsync(i => i.LibraryCode == code);
        if (item == null) ViewBag.Message = "No item found for that code.";
        return View(item);
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Account()
    {
        var vm = new KioskAccountViewModel();
        var userId = _users.GetUserId(User);
        if (userId == null) return Challenge();
        var borrower = await _db.Borrowers.AsNoTracking().SingleOrDefaultAsync(b => b.IdentityUserId == userId);
        if (borrower == null) { vm.Message = "This account is not linked to a library membership. Please ask authorised staff for help."; return View(vm); }
        vm.FullName = borrower.FullName;
        vm.IsActive = borrower.IsActive;
        if (!borrower.IsActive) vm.Message = "Your library membership is inactive. Checkout is unavailable.";
        vm.Loans = await _db.Loans.AsNoTracking().Where(l => l.BorrowerId == borrower.Id && l.ReturnedDate == null)
            .Select(l => new KioskLoanRow { Name = l.Item.Name, LibraryCode = l.Item.LibraryCode,
                DueDate = l.DueDate, FineAmount = l.FineAmount }).ToListAsync();
        vm.OutstandingFines = await _db.Loans.AsNoTracking()
            .Where(l => l.BorrowerId == borrower.Id && !l.FinePaid).SumAsync(l => l.FineAmount);
        return View(vm);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(string? libraryCode)
    {
        var userId = _users.GetUserId(User);
        if (userId == null) return Challenge();
        var borrower = await _db.Borrowers.AsNoTracking().SingleOrDefaultAsync(b => b.IdentityUserId == userId);
        if (borrower == null) { TempData["KioskError"] = "Your login is not linked to a library membership."; return RedirectToAction(nameof(Account)); }
        if (!borrower.IsActive) { TempData["KioskError"] = "Your membership is inactive."; return RedirectToAction(nameof(Account)); }
        // Fail closed until the shared hold and transfer rules are integrated.
        if (!_configuration.GetValue<bool>("Kiosk:SharedBorrowingRulesIntegrated"))
        { TempData["KioskError"] = "Checkout is temporarily unavailable while holds and transfers are being integrated."; return RedirectToAction(nameof(Account)); }
        // Do not turn on the switch until the shared borrowing checks are added here.
        TempData["KioskError"] = "Checkout integration requires the shared borrowing service. No loan has been created.";
        return RedirectToAction(nameof(Account));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Finish()
    {
        await _signIn.SignOutAsync();
        Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
        Response.Headers.Pragma = "no-cache";
        return RedirectToAction(nameof(Index));
    }
}
