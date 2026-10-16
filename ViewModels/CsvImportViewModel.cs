using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.ViewModels;

public class CsvImportViewModel
{
    public List<Branch> Branches { get; set; } = new();
    public int BranchId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public int ImportedCopies { get; set; }
    public int ImportedRows { get; set; }
    public int SkippedRows { get; set; }
    public List<CsvImportRowResult> Rows { get; set; } = new();
}

public record CsvImportRowResult(int RowNumber, string Title, string Outcome, string Detail);
