using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public class Music : Item
    {
        [Required]
        [StringLength(100)]
        public string Artist { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Genre")]
        public string MusicGenre { get; set; } = string.Empty;

        [Range(1900, 2100)]
        [Display(Name = "Release Year")]
        public int ReleaseYear { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Format")]
        public string Format { get; set; } = string.Empty;
    }
}