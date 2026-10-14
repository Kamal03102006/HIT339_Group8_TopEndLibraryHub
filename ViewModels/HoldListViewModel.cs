using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.ViewModels;

public class HoldListViewModel
{
    public string? Search { get; set; }
    public HoldStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageCount { get; set; } = 1;
    public int TotalCount { get; set; }
    public List<HoldListRow> Rows { get; set; } = new();
}

public class HoldListRow
{
    public int Id { get; set; }
    public string LibraryCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public ItemStatus CurrentItemStatus { get; set; }
    public string MembershipNumber { get; set; } = string.Empty;
    public string MemberName { get; set; } = string.Empty;
    public string PickupBranchName { get; set; } = string.Empty;
    public bool MemberIsActive { get; set; }
    public bool PickupBranchIsActive { get; set; }
    public HoldStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime CreatedAtDarwin { get; set; }
    public int? QueuePosition { get; set; }

    public bool CanCancel => Status == HoldStatus.Waiting;

    public string StatusLabel => Status switch
    {
        HoldStatus.Waiting => "Waiting",
        HoldStatus.Allocated => "Allocated",
        HoldStatus.ReadyForPickup => "Ready for pickup",
        HoldStatus.Fulfilled => "Collected",
        HoldStatus.Cancelled => "Cancelled",
        _ => "Needs review"
    };

    public string NextStep => Status switch
    {
        HoldStatus.Cancelled => "Request cancelled; it no longer occupies the queue.",
        HoldStatus.Fulfilled => "Item collected by the member.",
        HoldStatus.ReadyForPickup => "Collect from the selected pickup branch.",
        HoldStatus.Allocated when CurrentItemStatus == ItemStatus.InTransit =>
            "Reserved for this member and travelling to the pickup branch.",
        HoldStatus.Allocated => "Reserved; staff need to complete the pickup handover.",
        HoldStatus.Waiting when !MemberIsActive =>
            "Paused while the membership is inactive.",
        HoldStatus.Waiting when !PickupBranchIsActive =>
            "Paused while the pickup branch is inactive; ask staff for help.",
        HoldStatus.Waiting when CurrentItemStatus == ItemStatus.Damaged =>
            "Awaiting repair before the next eligible request can be allocated.",
        HoldStatus.Waiting when CurrentItemStatus == ItemStatus.Borrowed =>
            "Awaiting return before the next eligible request can be allocated.",
        HoldStatus.Waiting when CurrentItemStatus == ItemStatus.InTransit =>
            "Awaiting arrival before the next eligible request can be allocated.",
        HoldStatus.Waiting when CurrentItemStatus == ItemStatus.Destroyed =>
            "Item withdrawn; staff need to review this request.",
        _ => "Waiting for staff allocation."
    };
}
