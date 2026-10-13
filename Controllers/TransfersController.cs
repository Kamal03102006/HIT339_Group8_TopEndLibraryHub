using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers
{
    [Authorize(Roles = "Admin,Reception")]
    public class TransfersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransfersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var transfers = await _context.ItemTransfers
                .AsNoTracking()
                .Include(t => t.Item)
                .Include(t => t.OriginBranch)
                .Include(t => t.DestinationBranch)
                .OrderByDescending(t => t.DispatchedAtUtc)
                .ThenByDescending(t => t.Id)
                .ToListAsync();

            return View(transfers);
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

            var branchId = HttpContext.Session.GetInt32(
                "StaffBranchId");

            var deskId = HttpContext.Session.GetInt32(
                "StaffDeskId");

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

        private async Task PopulateTransferOptionsAsync(
            CreateTransferViewModel model,
            ReceptionDesk desk)
        {
            model.OriginLocation =
                $"{desk.Branch!.Name} / {desk.DeskCode}";

            model.Items = await _context.Items
                .AsNoTracking()
                .Where(i =>
                    i.BranchId == desk.BranchId &&
                    i.Status == ItemStatus.Available &&
                    !_context.Loans.Any(l =>
                        l.ItemId == i.Id &&
                        l.ReturnedDate == null) &&
                    !_context.ItemTransfers.Any(t =>
                        t.ItemId == i.Id &&
                        t.Status == TransferStatus.InTransit))
                .OrderBy(i => i.LibraryCode)
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.LibraryCode + " / " + i.Name
                })
                .ToListAsync();

            model.DestinationBranches = await _context.Branches
                .AsNoTracking()
                .Where(b =>
                    b.IsActive &&
                    b.Id != desk.BranchId)
                .OrderBy(b => b.Name)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var desk = await GetWorkingDeskAsync();

            if (desk is null)
            {
                TempData["LocationMessage"] =
                    "Select your working location before dispatching.";

                return RedirectToAction("Index", "StaffLocation");
            }

            var model = new CreateTransferViewModel();

            await PopulateTransferOptionsAsync(model, desk);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateTransferViewModel model)
        {
            var desk = await GetWorkingDeskAsync();

            if (desk is null)
            {
                TempData["LocationMessage"] =
                    "Select your working location before dispatching.";

                return RedirectToAction("Index", "StaffLocation");
            }

            if (!ModelState.IsValid)
            {
                await PopulateTransferOptionsAsync(model, desk);
                return View(model);
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                var destinationExists = await _context.Branches
                    .AnyAsync(b =>
                        b.Id == model.DestinationBranchId &&
                        b.IsActive);

                if (!destinationExists ||
                    model.DestinationBranchId == desk.BranchId)
                {
                    ModelState.AddModelError(
                        nameof(model.DestinationBranchId),
                        "Choose a different active destination branch.");
                }

                var item = await _context.Items
                    .SingleOrDefaultAsync(i =>
                        i.Id == model.ItemId);

                if (item is null ||
                    item.BranchId != desk.BranchId)
                {
                    ModelState.AddModelError(
                        nameof(model.ItemId),
                        "Select an item located at your working branch.");
                }
                else
                {
                    var onLoan = await _context.Loans.AnyAsync(l =>
                        l.ItemId == item.Id &&
                        l.ReturnedDate == null);

                    var activeTransfer =
                        await _context.ItemTransfers.AnyAsync(t =>
                            t.ItemId == item.Id &&
                            t.Status == TransferStatus.InTransit);

                    if (item.Status != ItemStatus.Available ||
                        onLoan ||
                        activeTransfer)
                    {
                        ModelState.AddModelError(
                            nameof(model.ItemId),
                            "The item cannot be dispatched. " +
                            "It must be available, with no active loan or transfer.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    await transaction.RollbackAsync();

                    await PopulateTransferOptionsAsync(model, desk);

                    return View(model);
                }

                var transfer = new ItemTransfer
                {
                    ItemId = item!.Id,
                    OriginBranchId = desk.BranchId,
                    DestinationBranchId = model.DestinationBranchId,
                    DispatchDeskId = desk.Id,
                    Status = TransferStatus.InTransit,
                    DispatchedAtUtc = DateTime.UtcNow,
                    DispatchedByUserId =
                        User.FindFirstValue(
                            ClaimTypes.NameIdentifier)!
                };

                item.Status = ItemStatus.InTransit;

                // Keep the origin branch until destination receipt.
                _context.ItemTransfers.Add(transfer);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] =
                    $"{item.LibraryCode} dispatched. " +
                    "Checkout is blocked until it is received.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                _context.ChangeTracker.Clear();

                ModelState.AddModelError(
                    string.Empty,
                    "The transfer could not be saved. " +
                    "Refresh the page and check whether another staff member " +
                    "has already dispatched the item.");

                await PopulateTransferOptionsAsync(model, desk);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(int id)
        {
            var desk = await GetWorkingDeskAsync();

            if (desk is null)
            {
                TempData["LocationMessage"] =
                    "Select your working location before receiving.";

                return RedirectToAction("Index", "StaffLocation");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                var transfer = await _context.ItemTransfers
                    .Include(t => t.Item)
                    .SingleOrDefaultAsync(t => t.Id == id);

                if (transfer is null)
                {
                    return NotFound();
                }

                if (transfer.DestinationBranchId != desk.BranchId)
                {
                    TempData["ErrorMessage"] =
                        "Only the destination branch can receive this item. " +
                        "Select a desk at the destination branch.";

                    return RedirectToAction(nameof(Index));
                }

                if (transfer.Status == TransferStatus.Received)
                {
                    TempData["ErrorMessage"] =
                        "This transfer has already been received. " +
                        "No changes were made.";

                    return RedirectToAction(nameof(Index));
                }

                if (transfer.Status != TransferStatus.InTransit ||
                    transfer.Item is null ||
                    transfer.Item.Status != ItemStatus.InTransit ||
                    transfer.Item.BranchId != transfer.OriginBranchId)
                {
                    TempData["ErrorMessage"] =
                        "The item and transfer records do not match. " +
                        "Ask an administrator to check them before receiving.";

                    return RedirectToAction(nameof(Index));
                }

                var activeLoan = await _context.Loans.AnyAsync(l =>
                    l.ItemId == transfer.ItemId &&
                    l.ReturnedDate == null);

                if (activeLoan)
                {
                    TempData["ErrorMessage"] =
                        "This item has an active loan. " +
                        "Ask an administrator to resolve it before receiving.";

                    return RedirectToAction(nameof(Index));
                }

                transfer.Item.BranchId = desk.BranchId;

                // Unreserved-item workflow only.
                // Connect the shared hold rules before integration.
                transfer.Item.Status = ItemStatus.Available;

                transfer.Status = TransferStatus.Received;
                transfer.ReceivingDeskId = desk.Id;
                transfer.ReceivedAtUtc = DateTime.UtcNow;
                transfer.ReceivedByUserId =
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] =
                    $"{transfer.Item.LibraryCode} received at " +
                    $"{desk.Branch!.Name}.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                _context.ChangeTracker.Clear();

                TempData["ErrorMessage"] =
                    "Receipt could not be saved. Refresh the transfer list " +
                    "and check whether another staff member has received it.";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}