using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.ViewModels;

public sealed class KioskScanViewModel
{
    public string LibraryCode { get; set; } = string.Empty;
    public string KioskBranch { get; set; } = string.Empty;
    public Item? Item { get; set; }
    public string? Message { get; set; }
    public string Availability { get; set; } = string.Empty;
    public bool CanCheckout { get; set; }
}
