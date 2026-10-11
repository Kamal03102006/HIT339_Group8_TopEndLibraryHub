using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Services
{
    public class HoldQueueService
    {
        private readonly ApplicationDbContext _context;

        public HoldQueueService(ApplicationDbContext context)
        {
            _context = context;
        }

        private IOrderedQueryable<Hold> WaitingQueue(int itemId)
        {
            return _context.Holds
                .Where(hold =>
                    hold.ItemId == itemId &&
                    hold.Status == HoldStatus.Waiting &&
                    hold.Borrower.IsActive)
                .OrderBy(hold => hold.CreatedAtUtc)
                .ThenBy(hold => hold.Id);
        }

        public Task<List<Hold>> GetWaitingQueueAsync(
            int itemId,
            CancellationToken cancellationToken = default)
        {
            return WaitingQueue(itemId)
                .AsNoTracking()
                .Include(hold => hold.Borrower)
                .Include(hold => hold.Item)
                .ToListAsync(cancellationToken);
        }

        // Finds a candidate; allocation will recheck availability.
        public Task<int?> GetNextWaitingHoldIdAsync(
            int itemId,
            CancellationToken cancellationToken = default)
        {
            return WaitingQueue(itemId)
                .Select(hold => (int?)hold.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int?> GetQueuePositionAsync(
            int itemId,
            int holdId,
            CancellationToken cancellationToken = default)
        {
            var holdIds = await WaitingQueue(itemId)
                .Select(hold => hold.Id)
                .ToListAsync(cancellationToken);

            var index = holdIds.IndexOf(holdId);
            return index < 0 ? null : index + 1;
        }
    }
}