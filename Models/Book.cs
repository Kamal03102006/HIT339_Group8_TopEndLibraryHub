using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public class Book : Item
    {
        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Genre")]
        public string BookGenre { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "ISBN")]
        public string? Isbn { get; set; }

        [Range(1000, 2100)]
        [Display(Name = "Publication Year")]
        public int PublicationYear { get; set; }

        [StringLength(100)]
        public string? Publisher { get; set; }
    }
}