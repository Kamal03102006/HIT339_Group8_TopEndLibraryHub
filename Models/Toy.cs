using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public class Toy : Item
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Toy Type")]
        public string ToyType { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [Display(Name = "Recommended Age")]
        public string RecommendedAge { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Manufacturer { get; set; }

        [StringLength(50)]
        public string? Material { get; set; }
    }
}