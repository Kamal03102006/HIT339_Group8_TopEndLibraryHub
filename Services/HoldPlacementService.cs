using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Services;

public sealed record HoldPlacementError(string Field, string Message);

public sealed record HoldPlacementResult(bool Succeeded, string Message,
    IReadOnlyList<HoldPlacementError> Errors)
{
    public static HoldPlacementResult Failure(string field, string message) =>
        new(false, string.Empty, new[] { new HoldPlacementError(field, message) });
}

// Staff and patrons use the same transaction, eligibility checks and queue order.
public sealed class HoldPlacementService
{
    private readonly ApplicationDbContext _db;
    private readonly LibraryClock _clock;
    private readonly HoldWorkflowService _workflow;

    public HoldPlacementService(ApplicationDbContext db, LibraryClock clock,
        HoldWorkflowService workflow)
    {
        _db = db;
        _clock = clock;
        _workflow = workflow;
    }

    public Task<HoldPlacementResult> PlaceForStaffAsync(string libraryCode,
        string membershipNumber, int pickupBranchId,
        CancellationToken cancellationToken = default) =>
        PlaceAsync(libraryCode, membershipNumber.Trim().ToUpperInvariant(), null,
            pickupBranchId, cancellationToken);

    // identityUserId is supplied by the controller's authenticated principal,
    // never by a membership number, borrower ID or hidden field in the form.
    public Task<HoldPlacementResult> PlaceForPatronAsync(string libraryCode,
        string identityUserId, int pickupBranchId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(identityUserId))
            return Task.FromResult(HoldPlacementResult.Failure(string.Empty,
                "Sign in with your linked library account first."));
        return PlaceAsync(libraryCode, null, identityUserId, pickupBranchId, cancellationToken);
    }

    private async Task<HoldPlacementResult> PlaceAsync(string libraryCode,
        string? membershipNumber, string? identityUserId, int pickupBranchId,
        CancellationToken cancellationToken)
    {
        var code = libraryCode.Trim().ToUpperInvariant();
        var itemId = await _db.Items.AsNoTracking()
            .Where(item => item.LibraryCode == code).Select(item => (int?)item.Id)
            .SingleOrDefaultAsync(cancellationToken);
        if (!itemId.HasValue)
            return HoldPlacementResult.Failure("LibraryCode",
                "No item was found with this library code.");

        try
        {
            await using var transaction = await _db.Database.BeginTransactionAsync(
                IsolationLevel.Serializable, cancellationToken);
            await _workflow.LockItemAsync(itemId.Value, cancellationToken);
            var item = await _db.Items.AsNoTracking().SingleOrDefaultAsync(
                item => item.Id == itemId.Value && item.LibraryCode == code, cancellationToken);
            var members = _db.Borrowers.AsNoTracking();
            var borrower = identityUserId is not null
                ? await members.SingleOrDefaultAsync(member =>
                    member.IdentityUserId == identityUserId, cancellationToken)
                : await members.SingleOrDefaultAsync(member =>
                    member.MembershipNumber == membershipNumber, cancellationToken);
            var branch = await _db.Branches.AsNoTracking().SingleOrDefaultAsync(
                branch => branch.Id == pickupBranchId && branch.IsActive, cancellationToken);
            var errors = new List<HoldPlacementError>();
            var memberField = identityUserId is null ? "MembershipNumber" : string.Empty;

            if (item is null)
                errors.Add(new("LibraryCode", "No item was found with this library code."));
            else if (item.Status is not (ItemStatus.Borrowed or ItemStatus.Damaged) ||
                await _db.ItemTransfers.AnyAsync(transfer => transfer.ItemId == item.Id &&
                    transfer.Status == TransferStatus.InTransit, cancellationToken))
                errors.Add(new("LibraryCode",
                    "Place a hold on an item that is borrowed or awaiting repair."));

            if (borrower is null)
                errors.Add(new(memberField, identityUserId is null
                    ? "No member was found with this membership number."
                    : "This login is not linked to a library membership. Ask staff for help."));
            else if (!borrower.IsActive)
                errors.Add(new(memberField,
                    "This membership is inactive. Ask staff to review it first."));

            if (branch is null)
                errors.Add(new("PickupBranchId", "Choose an active pickup branch."));

            if (item is not null && borrower is not null)
            {
                if (await _db.Holds.AnyAsync(hold => hold.ItemId == item.Id &&
                    hold.BorrowerId == borrower.Id &&
                    (hold.Status == HoldStatus.Waiting || hold.Status == HoldStatus.Allocated ||
                     hold.Status == HoldStatus.ReadyForPickup), cancellationToken))
                    errors.Add(new("LibraryCode",
                        "This member already has an active request for this item."));
                if (await _db.Loans.AnyAsync(loan => loan.ItemId == item.Id &&
                    loan.BorrowerId == borrower.Id && loan.ReturnedDate == null, cancellationToken))
                    errors.Add(new("LibraryCode", "This member already has this item on loan."));
            }

            if (errors.Count > 0)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new(false, string.Empty, errors);
            }

            _db.Holds.Add(new Hold
            {
                ItemId = item!.Id,
                BorrowerId = borrower!.Id,
                PickupBranchId = branch!.Id,
                CreatedAtUtc = _clock.UtcNow,
                Status = HoldStatus.Waiting
            });
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            // Joining a queue does not mean the item is ready. The existing
            // return/repair/transfer workflow produces the pickup notification.
            return new(true,
                $"Request placed for {borrower.MembershipNumber}: {item.LibraryCode}. " +
                $"Pickup branch: {branch.Name}.", Array.Empty<HoldPlacementError>());
        }
        catch (DbUpdateException)
        {
            _db.ChangeTracker.Clear();
            return HoldPlacementResult.Failure(string.Empty,
                "The request could not be saved. Check whether it already exists, " +
                "then refresh and try again.");
        }
        catch (SqlException exception) when (exception.Number is 1205 or 1222 or 51001)
        {
            _db.ChangeTracker.Clear();
            return HoldPlacementResult.Failure(string.Empty,
                "Another action is updating this item. Check your requests, then try again.");
        }
    }
}
