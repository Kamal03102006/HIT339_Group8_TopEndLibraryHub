using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public enum NotificationChannel
    {
        Email = 1,
        Sms = 2
    }

    public enum NotificationKind
    {
        Borrowed = 1,
        DueSoon = 2,
        FineAccruing = 3,
        HoldReady = 4
    }

    // A stored simulation record displayed on the staff dashboard.
    public class NotificationLog
    {
        public int Id { get; set; }

        public int BorrowerId { get; set; }
        public Borrower Borrower { get; set; } = null!;

        public int? LoanId { get; set; }
        public Loan? Loan { get; set; }

        public int? HoldId { get; set; }
        public Hold? Hold { get; set; }

        public NotificationChannel Channel { get; set; }
            = NotificationChannel.Email;

        public NotificationKind Kind { get; set; }
            = NotificationKind.Borrowed;

        [Required]
        [StringLength(180)]
        public string EventKey { get; set; } = string.Empty;

        [Required]
        [StringLength(254)]
        public string Recipient { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
