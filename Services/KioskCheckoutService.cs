using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Services
{
    public record KioskCheckoutResult(
        bool Success,
        string Message,
        DateTime? DueDate = null,
        string? ItemName = null
    );

    public class KioskCheckoutService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ILogger<KioskCheckoutService> _logger;

        public KioskCheckoutService(
            ApplicationDbContext db,
            IConfiguration configuration,
            ILogger<KioskCheckoutService> logger)
        {
            _db = db;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<KioskCheckoutResult> CheckoutAsync(
            string userId,
            string code)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new KioskCheckoutResult(
                    false,
                    "Please sign in before checkout.");
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return new KioskCheckoutResult(
                    false,
                    "Please scan or enter an item code.");
            }

            code = code.Trim().ToUpperInvariant();

            // Kiosk location must be configured.
            int kioskBranchId =
                _configuration.GetValue<int>("Kiosk:BranchId");

            if (kioskBranchId <= 0)
            {
                return new KioskCheckoutResult(
                    false,
                    "Kiosk location is not configured. Please contact staff.");
            }

            try
            {
                await using var transaction =
                    await _db.Database.BeginTransactionAsync(
                        IsolationLevel.Serializable);

                // Resolve the borrower through the
                // authenticated Identity account.
                var borrower = await _db.Borrowers
                    .SingleOrDefaultAsync(
                        b => b.IdentityUserId == userId);

                if (borrower == null)
                {
                    return new KioskCheckoutResult(
                        false,
                        "Your login is not linked to a library membership.");
                }

                if (!borrower.IsActive)
                {
                    return new KioskCheckoutResult(
                        false,
                        "Your membership is inactive.");
                }

                // Verify the configured kiosk branch.
                var kioskBranch = await _db.Branches
                    .SingleOrDefaultAsync(
                        b => b.Id == kioskBranchId);

                if (kioskBranch == null ||
                    !kioskBranch.IsActive)
                {
                    return new KioskCheckoutResult(
                        false,
                        "This kiosk's branch is unavailable.");
                }

                // Find scanned item.
                var item = await _db.Items
                    .SingleOrDefaultAsync(
                        i => i.LibraryCode == code);

                if (item == null)
                {
                    return new KioskCheckoutResult(
                        false,
                        "Item not found.");
                }

                if (item.Status != ItemStatus.Available)
                {
                    return new KioskCheckoutResult(
                        false,
                        "This item is unavailable for borrowing.");
                }

                // Physical branch restriction.
                if (item.BranchId != kioskBranchId)
                {
                    return new KioskCheckoutResult(
                        false,
                        "This item belongs to another branch. Please visit reception.");
                }

                // Prevent duplicate loans.
                bool hasActiveLoan = await _db.Loans
                    .AnyAsync(l =>
                        l.ItemId == item.Id &&
                        l.ReturnedDate == null);

                if (hasActiveLoan)
                {
                    return new KioskCheckoutResult(
                        false,
                        "This item is already on loan.");
                }

                // Block transfers.
                bool hasActiveTransfer =
                    await _db.ItemTransfers.AnyAsync(t =>
                        t.ItemId == item.Id &&
                        t.Status == TransferStatus.InTransit);

                if (hasActiveTransfer)
                {
                    return new KioskCheckoutResult(
                        false,
                        "This item is currently being transferred.");
                }

                // Check active reservations.
                var activeHolds = await _db.Holds
                    .Where(h =>
                        h.ItemId == item.Id &&
                        (
                            h.Status == HoldStatus.Waiting ||
                            h.Status == HoldStatus.Allocated ||
                            h.Status == HoldStatus.ReadyForPickup
                        ))
                    .OrderBy(h => h.CreatedAtUtc)
                    .ThenBy(h => h.Id)
                    .ToListAsync();

                Hold? readyHold = null;

                if (activeHolds.Count > 0)
                {
                    // Do not bypass waiting reservations.
                    if (activeHolds.Any(h =>
                        h.Status == HoldStatus.Waiting))
                    {
                        return new KioskCheckoutResult(
                            false,
                            "This item has a waiting reservation.");
                    }

                    if (activeHolds.Count != 1)
                    {
                        return new KioskCheckoutResult(
                            false,
                            "This item has reservations requiring staff assistance.");
                    }

                    readyHold = activeHolds[0];

                    bool validPickup =
                        readyHold.BorrowerId == borrower.Id &&
                        readyHold.Status == HoldStatus.ReadyForPickup &&
                        readyHold.ReadyForPickupAtUtc.HasValue &&
                        readyHold.PickupBranchId == kioskBranchId;

                    if (!validPickup)
                    {
                        return new KioskCheckoutResult(
                            false,
                            "This item is reserved or not ready for pickup.");
                    }
                }

                // Create actual loan.
                DateTime borrowedDate = DateTime.UtcNow;
                DateTime dueDate = borrowedDate.Date.AddDays(14);

                var loan = new Loan
                {
                    ItemId = item.Id,
                    BorrowerId = borrower.Id,
                    BorrowedDate = borrowedDate,
                    DueDate = dueDate,
                    ReturnedDate = null,
                    FineAmount = 0,
                    FinePaid = false
                };

                _db.Loans.Add(loan);

                // Update catalogue status.
                item.Status = ItemStatus.Borrowed;

                // Fulfil an eligible pickup reservation.
                if (readyHold != null)
                {
                    readyHold.Status = HoldStatus.Fulfilled;
                    readyHold.FulfilledAtUtc = borrowedDate;
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return new KioskCheckoutResult(
                    true,
                    "Checkout successful.",
                    dueDate,
                    item.Name);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Kiosk checkout concurrency conflict.");

                return new KioskCheckoutResult(
                    false,
                    "The item changed during checkout. Please try again.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Kiosk checkout database conflict.");

                return new KioskCheckoutResult(
                    false,
                    "Checkout could not be completed. Please contact reception.");
            }
        }
    }
}