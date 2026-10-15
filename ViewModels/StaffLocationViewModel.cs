using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TopEndLibraryHub.ViewModels
{
    public class StaffLocationViewModel
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Select a branch.")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Select a reception desk.")]
        [Display(Name = "Reception desk")]
        public int ReceptionDeskId { get; set; }

        public List<SelectListItem> Branches { get; set; } = new();
        public List<SelectListItem> Desks { get; set; } = new();

        public string? CurrentLocation { get; set; }
    }
}