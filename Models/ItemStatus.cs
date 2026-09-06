using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public enum ItemStatus
    {
        [Display(Name = "Available")]
        Available = 1,

        [Display(Name = "Borrowed")]
        Borrowed = 2,

        [Display(Name = "Damaged")]
        Damaged = 3,

        [Display(Name = "Destroyed")]
        Destroyed = 4
    }
}