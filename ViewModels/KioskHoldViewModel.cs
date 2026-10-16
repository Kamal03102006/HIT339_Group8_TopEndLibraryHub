using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.ViewModels;

public sealed class KioskHoldViewModel
{
    [Required]
    [StringLength(20)]
    [Display(Name = "Library code")]
    [RegularExpression(@"^[A-Za-z]{2,4}-[0-9]{4}$",
        ErrorMessage = "Use an item code such as BK-0001 or TY-0001.")]
    public string LibraryCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Choose a pickup branch.")]
    [Range(1, int.MaxValue, ErrorMessage = "Choose a valid pickup branch.")]
    [Display(Name = "Pickup branch")]
    public int? PickupBranchId { get; set; }

    [BindNever, ValidateNever]
    public List<SelectListItem> PickupBranches { get; set; } = new();

    [BindNever, ValidateNever]
    public string MemberName { get; set; } = string.Empty;

    [BindNever, ValidateNever]
    public string MembershipNumber { get; set; } = string.Empty;

    [BindNever, ValidateNever]
    public string? ItemName { get; set; }

    [BindNever, ValidateNever]
    public ItemStatus? ItemStatus { get; set; }
}
