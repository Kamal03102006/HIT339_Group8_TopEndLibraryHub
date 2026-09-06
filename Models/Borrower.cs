using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public class Borrower
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Membership Number")]
        [RegularExpression(
            @"^MBR-\d{4}$",
            ErrorMessage = "Use the format MBR-0001.")]
        public string MembershipNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(25)]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Date Registered")]
        [DataType(DataType.Date)]
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

        [Display(Name = "Active Member")]
        public bool IsActive { get; set; } = true;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}