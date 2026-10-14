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

        public KioskCheckoutService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<KioskCheckoutResult> CheckoutAsync(
            string userId,
            string code)
        {
            // 1. Verify login.
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new KioskCheckoutResult(
                    false,
                    "Please sign in to use the kiosk.");
            }

            // 2. Find the linked borrower.
            var borrower = await _db.Borrowers
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    b => b.IdentityUserId == userId);

            if (borrower == null)
            {
                return new KioskCheckoutResult(
                    false,
                    "Your login is not linked to a library membership.");
            }

            // 3. Verify membership.
            if (!borrower.IsActive)
            {
                return new KioskCheckoutResult(
                    false,
                    "Your library membership is inactive.");
            }

            // 4. Validate item code.
            if (string.IsNullOrWhiteSpace(code))
            {
                return new KioskCheckoutResult(
                    false,
                    "Please enter or scan an item code.");
            }

            code = code.Trim().ToUpperInvariant();

            // 5. Find item.
            var item = await _db.Items
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    i => i.LibraryCode == code);

            if (item == null)
            {
                return new KioskCheckoutResult(
                    false,
                    "Item not found.");
            }

            // 6. Check item status.
            if (item.Status != ItemStatus.Available)
            {
                return new KioskCheckoutResult(
                    false,
                    "This item is not available for borrowing.");
            }

            // 7. Check active loans.
            var alreadyBorrowed = await _db.Loans
                .AsNoTracking()
                .AnyAsync(l =>
                    l.ItemId == item.Id &&
                    l.ReturnedDate == null);

            if (alreadyBorrowed)
            {
                return new KioskCheckoutResult(
                    false,
                    "This item is already borrowed.");
            }

            // 8. Check existing holds.
            var hasActiveHold = await _db.Holds
                .AsNoTracking()
                .AnyAsync(h =>
                    h.ItemId == item.Id &&
                    (
                        h.Status == HoldStatus.Waiting ||
                        h.Status == HoldStatus.Allocated ||
                        h.Status == HoldStatus.ReadyForPickup
                    ));

            if (hasActiveHold)
            {
                return new KioskCheckoutResult(
                    false,
                    "This item has an active reservation. Please contact reception.");
            }

            // 9. Safety restriction.
            //
            // The current ApplicationDbContext does not
            // include Branches or ItemTransfers.
            //
            // Checkout must not create a loan until those
            // checks are integrated.

            return new KioskCheckoutResult(
                false,
                "Self-service checkout is temporarily unavailable while branch and transfer checks are being integrated. Please contact reception.");
        }
    }
}