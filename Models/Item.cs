using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public abstract class Item
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Library Code")]
        [RegularExpression(
            @"^[A-Z]{2,4}-\d{4}$",
            ErrorMessage = "Use 2 to 4 capital letters, a hyphen and 4 numbers, for example BK-0001.")]
        public string LibraryCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500, MinimumLength = 10)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public ItemStatus Status { get; set; } = ItemStatus.Available;

        [Display(Name = "Date Added")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}