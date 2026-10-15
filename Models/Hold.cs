using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public class Hold
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int ItemId { get; set; }

        public Item Item { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int BorrowerId { get; set; }

        public Borrower Borrower { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int PickupBranchId { get; set; }

        public DateTime CreatedAtUtc { get; set; }
            = DateTime.UtcNow;

        public HoldStatus Status { get; set; }
            = HoldStatus.Waiting;

        public DateTime? AllocatedAtUtc { get; set; }

        public DateTime? ReadyForPickupAtUtc { get; set; }

        public DateTime? FulfilledAtUtc { get; set; }

        public DateTime? CancelledAtUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
            = Array.Empty<byte>();
    }
}