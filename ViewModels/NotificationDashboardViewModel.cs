using Microsoft.AspNetCore.Mvc.Rendering;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.ViewModels
{
    public class NotificationDashboardViewModel
    {
        public NotificationKind? Kind { get; set; }
        public NotificationChannel? Channel { get; set; }
        public string Search { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public int TotalRecords { get; set; }
        public List<NotificationRecordViewModel> Records { get; set; } = new();

        public int TotalPages => Math.Max(1,
            (int)Math.Ceiling(TotalRecords / (double)PageSize));
        public int FirstRecord => TotalRecords == 0 ? 0 : (Page - 1) * PageSize + 1;
        public int LastRecord => Math.Min(Page * PageSize, TotalRecords);
        public bool HasFilters => Kind.HasValue || Channel.HasValue || Search.Length > 0;

        public List<SelectListItem> KindOptions { get; } = new()
        {
            new SelectListItem("Loan confirmed", "1"),
            new SelectListItem("Due soon", "2"),
            new SelectListItem("Fine accruing", "3"),
            new SelectListItem("Ready for pickup", "4")
        };

        public List<SelectListItem> ChannelOptions { get; } = new()
        {
            new SelectListItem("Email", "1"),
            new SelectListItem("SMS", "2")
        };
    }

    public class NotificationRecordViewModel
    {
        public DateTime CreatedAtUtc { get; set; }
        public NotificationKind Kind { get; set; }
        public NotificationChannel Channel { get; set; }
        public string MembershipNumber { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public string KindLabel => Kind switch
        {
            NotificationKind.Borrowed => "Loan confirmed",
            NotificationKind.DueSoon => "Due soon",
            NotificationKind.FineAccruing => "Fine accruing",
            NotificationKind.HoldReady => "Ready for pickup",
            _ => "Other"
        };

        public string ChannelLabel => Channel switch
        {
            NotificationChannel.Email => "Email",
            NotificationChannel.Sms => "SMS",
            _ => "Other"
        };
    }
}
