using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public class ItemTransfer
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public int OriginBranchId { get; set; }
        public Branch? OriginBranch { get; set; }

        public int DestinationBranchId { get; set; }
        public Branch? DestinationBranch { get; set; }

        public int DispatchDeskId { get; set; }
        public ReceptionDesk? DispatchDesk { get; set; }

        public int? ReceivingDeskId { get; set; }
        public ReceptionDesk? ReceivingDesk { get; set; }

        public TransferStatus Status { get; set; }
            = TransferStatus.InTransit;

        public DateTime DispatchedAtUtc { get; set; }

        public DateTime? ReceivedAtUtc { get; set; }

        [Required]
        [StringLength(450)]
        public string DispatchedByUserId { get; set; }
            = string.Empty;

        [StringLength(450)]
        public string? ReceivedByUserId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}