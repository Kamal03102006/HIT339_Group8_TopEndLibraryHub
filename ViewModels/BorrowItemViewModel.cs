using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.ViewModels
{
    public class BorrowItemViewModel
    {
        [Required]
        [Display(Name = "Library Code")]
        [RegularExpression(
            @"^[A-Z]{2,4}-\d{4}$",
            ErrorMessage = "Enter a valid code such as BK-0001.")]
        public string LibraryCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Membership Number")]
        [StringLength(20)]
        public string MembershipNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } =
            DateTime.UtcNow.Date.AddDays(14);
    }
}