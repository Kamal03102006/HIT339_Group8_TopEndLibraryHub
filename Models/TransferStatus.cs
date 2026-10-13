using System.ComponentModel.DataAnnotations;

namespace TopEndLibraryHub.Models
{
    public enum TransferStatus
    {
        [Display(Name = "In transit")]
        InTransit = 1,

        [Display(Name = "Received")]
        Received = 2
    }
}