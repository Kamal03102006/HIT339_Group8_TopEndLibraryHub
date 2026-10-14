using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public class ReceptionDesk
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Desk Code")]
        public string DeskCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue,
            ErrorMessage = "Select a valid branch.")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        public Branch? Branch { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}