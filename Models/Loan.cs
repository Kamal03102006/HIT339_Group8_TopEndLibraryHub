using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopEndLibraryHub.Models
{
    public class Loan : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Item")]
        public int ItemId { get; set; }

        public Item Item { get; set; } = null!;

        [Required]
        [Display(Name = "Borrower")]
        public int BorrowerId { get; set; }

        public Borrower Borrower { get; set; } = null!;

        [Display(Name = "Borrowed Date")]
        [DataType(DataType.Date)]
        public DateTime BorrowedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(14);

        [Display(Name = "Returned Date")]
        [DataType(DataType.Date)]
        public DateTime? ReturnedDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 100000)]
        [Display(Name = "Fine Amount")]
        public decimal FineAmount { get; set; }

        [Display(Name = "Fine Paid")]
        public bool FinePaid { get; set; }

        [Display(Name = "Fine Paid Date")]
        [DataType(DataType.Date)]
        public DateTime? FinePaidDate { get; set; }

        [NotMapped]
        public bool IsReturned => ReturnedDate.HasValue;

        [NotMapped]
        public bool IsOverdue =>
            !ReturnedDate.HasValue && DueDate.Date < DateTime.UtcNow.Date;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (DueDate.Date <= BorrowedDate.Date)
            {
                yield return new ValidationResult(
                    "The due date must be after the borrowed date.",
                    new[] { nameof(DueDate) });
            }

            if (ReturnedDate.HasValue &&
                ReturnedDate.Value.Date < BorrowedDate.Date)
            {
                yield return new ValidationResult(
                    "The returned date cannot be before the borrowed date.",
                    new[] { nameof(ReturnedDate) });
            }

            if (FinePaid && FineAmount <= 0)
            {
                yield return new ValidationResult(
                    "A fine can only be marked as paid when a fine amount exists.",
                    new[] { nameof(FinePaid), nameof(FineAmount) });
            }
        }
    }
}