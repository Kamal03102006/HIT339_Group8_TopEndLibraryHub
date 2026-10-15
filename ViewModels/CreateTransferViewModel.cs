using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TopEndLibraryHub.ViewModels
{
    public class CreateTransferViewModel
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Select an item.")]
        [Display(Name = "Item to transfer")]
        public int ItemId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Select a destination branch.")]
        [Display(Name = "Destination branch")]
        public int DestinationBranchId { get; set; }

        public string OriginLocation { get; set; } = string.Empty;

        public List<SelectListItem> Items { get; set; } = new();

        public List<SelectListItem> DestinationBranches { get; set; }
            = new();
    }
}