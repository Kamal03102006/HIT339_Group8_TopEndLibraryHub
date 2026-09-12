using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.ViewModels
{
    public class ReturnItemViewModel
    {
        [Required]
        [Display(Name = "Library Code")]
        [RegularExpression(
            @"^[A-Z]{2,4}-\d{4}$",
            ErrorMessage = "Enter a valid code such as BK-0001.")]
        public string LibraryCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Return Date")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; } =
            DateTime.UtcNow.Date;

        [Display(Name = "Pay Fine Now")]
        public bool PayFineNow { get; set; }
    }
}