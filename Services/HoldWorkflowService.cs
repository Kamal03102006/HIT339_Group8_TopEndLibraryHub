using System.Data;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Services;

public class HoldWorkflowService
{
    private readonly ApplicationDbContext _context;
    private readonly LibraryClock _clock;
    private readonly NotificationService _notifications;
    private readonly ILogger<HoldWorkflowService> _logger;
    private readonly HashSet<int> _lockedItems = new();
    private Guid? _lockTransactionId;

    public HoldWorkflowService(ApplicationDbContext context, LibraryClock clock,
        NotificationService notifications, ILogger<HoldWorkflowService> logger)
    {
        _context = context;
        _clock = clock;
        _notifications = notifications;
        _logger = logger;
    }

    public async Task LockItemAsync(int itemId,
        CancellationToken cancellationToken = default)
    {
        var transaction = _context.Database.CurrentTransaction
            ?? throw new InvalidOperationException("An item operation requires a transaction.");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(itemId);

        if (_lockTransactionId != transaction.TransactionId)
        {
            _lockedItems.Clear();
            _lockTransactionId = transaction.TransactionId;
        }
        if (_lockedItems.Contains(itemId)) return;

        var resource = $"TopEndLibraryHub:item:{itemId}";
        await _context.Database.ExecuteSqlInterpolatedAsync($@"
            DECLARE @lockResult int;
            EXEC @lockResult = sys.sp_getapplock
                @Resource = {resource}, @LockMode = N'Exclusive',
                @LockOwner = N'Transaction', @LockTimeout = 5000;
            IF @lockResult < 0
                THROW 51001, 'This item is being updated. Refresh and try again.', 1;",
            cancellationToken);
        _lockedItems.Add(itemId);
    }

    // Call after obtaining the item lock, in the caller's existing transaction.
    // Save a return/receipt first so active-loan/transfer queries see that transition.
    public async Task<Hold?> RefreshLockedAsync(Item item,
        CancellationToken cancellationToken = default)
    {
        var transaction = _context.Database.CurrentTransaction;
        if (transaction is null || transaction.TransactionId != _lockTransactionId ||
            !_lockedItems.Contains(item.Id))
            throw new InvalidOperationException("Hold refresh requires the item transaction lock.");

        if (item.Status != ItemStatus.Available ||
            !await _context.Branches.AnyAsync(branch =>
                branch.Id == item.BranchId && branch.IsActive, cancellationToken) ||
            await _context.Loans.AnyAsync(loan =>
                loan.ItemId == item.Id && loan.ReturnedDate == null, cancellationToken) ||
            await _context.ItemTransfers.AnyAsync(transfer =>
                transfer.ItemId == item.Id && transfer.Status == TransferStatus.InTransit,
                cancellationToken))
            return null;

        var hold = await _context.Holds
            .Include(hold => hold.Borrower)
            .Include(hold => hold.PickupBranch)
            .SingleOrDefaultAsync(hold => hold.ItemId == item.Id &&
                (hold.Status == HoldStatus.Allocated ||
                 hold.Status == HoldStatus.ReadyForPickup), cancellationToken);

        if (hold is not null && (!hold.Borrower.IsActive || !hold.PickupBranch.IsActive))
        {
            // Pause eligibility, preserving the original queue timestamp/history.
            hold.Status = HoldStatus.Waiting;
            hold.AllocatedAtUtc = null;
            hold.ReadyForPickupAtUtc = null;
            await _context.SaveChangesAsync(cancellationToken);
            hold = null;
        }

        hold ??= await _context.Holds
            .Include(request => request.Borrower)
            .Include(request => request.PickupBranch)
            .Where(request => request.ItemId == item.Id &&
                request.Status == HoldStatus.Waiting && request.Borrower.IsActive &&
                request.PickupBranch.IsActive)
            .OrderBy(request => request.CreatedAtUtc)
            .ThenBy(request => request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (hold is null) return null;

        hold.AllocatedAtUtc ??= _clock.UtcNow;
        if (hold.PickupBranchId == item.BranchId)
        {
            if (hold.Status != HoldStatus.ReadyForPickup || !hold.ReadyForPickupAtUtc.HasValue)
                hold.ReadyForPickupAtUtc = _clock.UtcNow;
            hold.Status = HoldStatus.ReadyForPickup;
        }
        else
        {
            hold.Status = HoldStatus.Allocated;
            hold.ReadyForPickupAtUtc = null;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return hold;
    }

    public async Task ProcessAvailableItemsAsync(
        CancellationToken cancellationToken = default)
    {
        var itemIds = await _context.Items.AsNoTracking()
            .Where(item => item.Status == ItemStatus.Available &&
                _context.Holds.Any(hold => hold.ItemId == item.Id &&
                    (hold.Status == HoldStatus.Waiting || hold.Status == HoldStatus.Allocated ||
                     hold.Status == HoldStatus.ReadyForPickup)))
            .OrderBy(item => item.Id).Select(item => item.Id)
            .ToListAsync(cancellationToken);

        foreach (var itemId in itemIds)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _context.ChangeTracker.Clear();
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable, cancellationToken);
                await LockItemAsync(itemId, cancellationToken);
                var item = await _context.Items.SingleOrDefaultAsync(
                    item => item.Id == itemId, cancellationToken);
                if (item is not null) await RefreshLockedAsync(item, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                _context.ChangeTracker.Clear();
                _logger.LogWarning(exception,
                    "Hold allocation for item {ItemId} will be retried at the next check.", itemId);
            }
        }
    }

    // Notifications follow committed state. A failed simulation is retried by the worker.
    public async Task NotifyReadyAfterCommitAsync(Hold? hold,
        CancellationToken cancellationToken = default)
    {
        if (hold is null || hold.Status != HoldStatus.ReadyForPickup) return;
        try
        {
            await _notifications.RecordHoldReadyAsync(hold.Id, hold.PickupBranch.Name,
                cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception,
                "Pickup simulation for hold {HoldId} will be retried by the worker.", hold.Id);
        }
    }
}
