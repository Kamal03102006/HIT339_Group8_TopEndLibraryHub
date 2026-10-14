using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}