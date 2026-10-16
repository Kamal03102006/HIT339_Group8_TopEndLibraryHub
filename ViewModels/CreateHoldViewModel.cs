using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TopEndLibraryHub.ViewModels
{
    public class CreateHoldViewModel
    {
        [Required(ErrorMessage = "Scan or enter the item code.")]
        [StringLength(20)]
        [Display(Name = "Item code")]
        [RegularExpression(@"^[A-Za-z]{2,4}-[0-9]{4}$",
            ErrorMessage = "Use a code such as BK-0001 or TY-0001.")]
        public string LibraryCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Scan or enter the membership number.")]
        [StringLength(20)]
        [Display(Name = "Membership number")]
        [RegularExpression(@"^[Mm][Bb][Rr]-[0-9]{4}$",
            ErrorMessage = "Use a membership number such as MBR-0001.")]
        public string MembershipNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Choose a pickup branch.")]
        [Range(1, int.MaxValue, ErrorMessage = "Choose a valid pickup branch.")]
        [Display(Name = "Pickup branch")]
        public int? PickupBranchId { get; set; }

        [BindNever]
        [ValidateNever]
        public List<SelectListItem> PickupBranches { get; set; } = new();
    }
}
