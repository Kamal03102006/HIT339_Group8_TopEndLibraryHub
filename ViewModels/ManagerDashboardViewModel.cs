using System;
using System.Collections.Generic;

namespace TopEndLibraryHub.ViewModels
{
    public class ManagerDashboardViewModel
    {
        // Item statistics
        public int TotalItems { get; set; }
        public int AvailableItems { get; set; }
        public int InTransitItems { get; set; }
        public int ReservedItems { get; set; }
        public int BorrowedItems { get; set; }
        public int DamagedItems { get; set; }
        public int DestroyedItems { get; set; }

        // Loan statistics
        public int TotalLoans { get; set; }
        public int ActiveLoans { get; set; }
        public int OverdueLoans { get; set; }
        public int ReturnedLoans { get; set; }

        // Fine statistics
        public decimal TotalFines { get; set; }
        public decimal PaidFines { get; set; }
        public decimal OutstandingFines { get; set; }
        public decimal EstimatedCurrentLateCharges { get; set; }
        public DateTime GeneratedAtDarwin { get; set; }
        public List<FineAuditRow> FineAudit { get; set; } = new();
        public List<InventoryAuditRow> InventoryAudit { get; set; } = new();

        // Percentage of items currently available
        public double AvailabilityRate { get; set; }

        public List<ItemTypeStatisticViewModel> ItemsByType { get; set; }
            = new();

        public List<PopularItemViewModel> PopularItems { get; set; }
            = new();

        public List<RecentLoanViewModel> RecentLoans { get; set; }
            = new();
    }

    public class ItemTypeStatisticViewModel
    {
        public string ItemType { get; set; } = string.Empty;
        public int TotalItems { get; set; }
        public int AvailableItems { get; set; }
    }

    public class FineAuditRow
    {
        public int LoanId { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string MembershipNumber { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedAtDarwin { get; set; }
        public decimal FineAmount { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidAtDarwin { get; set; }
    }

    public class InventoryAuditRow
    {
        public string ItemCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string PhysicalStatus { get; set; } = string.Empty;
        public bool GenerallyAvailable { get; set; }
        public bool Reserved { get; set; }
        public bool InTransit { get; set; }
    }

    public class PopularItemViewModel
    {
        public int ItemId { get; set; }
        public string LibraryCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int LoanCount { get; set; }
    }

    public class RecentLoanViewModel
    {
        public int LoanId { get; set; }
        public string LibraryCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime BorrowedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public decimal FineAmount { get; set; }
        public bool FinePaid { get; set; }
    }
}
