using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Services;

public static class CirculationRules
{
    public static IQueryable<int> ReservedItemIds(ApplicationDbContext context) =>
        context.Holds.Where(hold =>
            hold.Status == HoldStatus.Allocated || hold.Status == HoldStatus.ReadyForPickup ||
            (hold.Status == HoldStatus.Waiting && hold.Borrower.IsActive &&
             hold.PickupBranch.IsActive)).Select(hold => hold.ItemId).Distinct();

    // General availability is shared by the catalogue, API and kiosk.
    // A physical Available status alone does not override a member's reservation.
    public static IQueryable<Item> GenerallyAvailable(
        ApplicationDbContext context, IQueryable<Item>? items = null)
    {
        var reservedIds = ReservedItemIds(context);
        return (items ?? context.Items).Where(item =>
            item.Status == ItemStatus.Available &&
            item.Branch != null && item.Branch.IsActive &&
            !context.Loans.Any(loan => loan.ItemId == item.Id &&
                loan.ReturnedDate == null) &&
            !context.ItemTransfers.Any(transfer => transfer.ItemId == item.Id &&
                transfer.Status == TransferStatus.InTransit) &&
            !reservedIds.Contains(item.Id));
    }
}
