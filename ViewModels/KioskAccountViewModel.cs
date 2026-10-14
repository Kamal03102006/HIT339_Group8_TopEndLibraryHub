namespace TopEndLibraryHub.ViewModels;
public sealed class KioskAccountViewModel
{
    public string? FullName { get; set; }
    public string? Message { get; set; }
    public bool IsActive { get; set; }
    public List<KioskLoanRow> Loans { get; set; } = new();
    public decimal OutstandingFines { get; set; }
}
public sealed class KioskLoanRow
{
    public string Name { get; set; } = "";
    public string LibraryCode { get; set; } = "";
    public DateTime DueDate { get; set; }
    public decimal FineAmount { get; set; }
}
