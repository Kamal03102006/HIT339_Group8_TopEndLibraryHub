using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.ViewModels;
public sealed class KioskAccountViewModel
{
    public string? FullName { get; set; }
    public string? MembershipNumber { get; set; }
    public string? Message { get; set; }
    public bool IsActive { get; set; }
    public List<KioskLoanRow> Loans { get; set; } = new();
    public decimal OutstandingFines { get; set; }
    public decimal EstimatedLateFines { get; set; }
    public List<KioskHoldRow> Holds { get; set; } = new();
}
public sealed class KioskLoanRow
{
    public string Name { get; set; } = "";
    public string LibraryCode { get; set; } = "";
    public DateTime DueDate { get; set; }
    public decimal FineAmount { get; set; }
    public decimal EstimatedLateFine { get; set; }
}

public sealed class KioskHoldRow
{
    public string Name { get; set; } = string.Empty;
    public string LibraryCode { get; set; } = string.Empty;
    public string PickupBranch { get; set; } = string.Empty;
    public HoldStatus Status { get; set; }
    public int? QueuePosition { get; set; }
    public bool EligibilityPaused { get; set; }
    public string StatusLabel => EligibilityPaused ? "Paused — ask staff" : Status switch
    {
        HoldStatus.ReadyForPickup => "Ready for collection",
        HoldStatus.Allocated => "Preparing for transfer",
        _ => "Waiting"
    };
}
