using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TopEndLibraryHub.Controllers;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Tests;

public class ReportingAndImportTests
{
    [Fact]
    public async Task Availability_ExcludesReservationsLoansTransfersAndInactiveBranches()
    {
        await using var db = TestData.NewContext(); await TestData.SeedReports(db);
        var report = await new ManagerReportingService(db, new(new FixedClock())).BuildAsync();
        Assert.Equal(13, report.TotalItems); Assert.Equal(3, report.AvailableItems);
        Assert.Equal(3, report.ReservedItems); Assert.Equal(2, report.InTransitItems);
        Assert.Equal(new[] { "BK-0001", "BK-0009", "BK-0010" },
            report.InventoryAudit.Where(row => row.GenerallyAvailable).Select(row => row.ItemCode));
        Assert.Equal(3, report.ItemsByType.Single().AvailableItems);
        Assert.False(report.InventoryAudit.Single(row => row.ItemCode == "BK-0002").GenerallyAvailable);
    }

    [Fact]
    public async Task RecordedRevenue_SeparatesUnpaidFinesFromAccruingEstimates()
    {
        await using var db = TestData.NewContext(); await TestData.SeedReports(db);
        var report = await new ManagerReportingService(db, new(new FixedClock())).BuildAsync();
        Assert.Equal(9m, report.TotalFines); Assert.Equal(3m, report.PaidFines);
        Assert.Equal(6m, report.OutstandingFines); Assert.Equal(1.50m, report.EstimatedCurrentLateCharges);
        Assert.Equal(report.TotalFines, report.PaidFines + report.OutstandingFines);
        Assert.Equal(new DateTime(2026, 10, 16, 0, 30, 0), report.FineAudit.Single(row => row.Paid).PaidAtDarwin);
    }

    [Fact]
    public async Task ControllerAndCsv_UseTheSameDefinitionsAndTraceableFineRows()
    {
        await using var db = TestData.NewContext(); await TestData.SeedReports(db);
        var controller = new ManagerController(new ManagerReportingService(db, new(new FixedClock())));
        var view = Assert.IsType<ViewResult>(await controller.Index());
        var model = Assert.IsType<ManagerDashboardViewModel>(view.Model);
        var file = Assert.IsType<FileContentResult>(await controller.ExportCsv());
        var csv = Encoding.UTF8.GetString(file.FileContents);
        Assert.Equal(3, model.AvailableItems);
        Assert.Contains("\"Generally Available Items\",\"3\"", csv);
        Assert.Contains("\"Recorded Paid Fines AUD\",\"3.00\"", csv);
        Assert.Contains("\"Recorded Unpaid Fines AUD\",\"6.00\"", csv);
        Assert.Contains("\"MBR-1003\"", csv); Assert.Contains("\"2026-10-16 00:30:00\"", csv);
        Assert.DoesNotContain("active@example.com", csv);
    }

    [Fact]
    public async Task PdfExport_ProducesARealReportFromTheSameSnapshot()
    {
        await using var db = TestData.NewContext(); await TestData.SeedReports(db);
        var report = await new ManagerReportingService(db, new(new FixedClock())).BuildAsync();
        var pdf = ManagerReportExport.Pdf(report);
        Assert.StartsWith("%PDF-", Encoding.ASCII.GetString(pdf, 0, 5));
        Assert.True(pdf.Length > 1000);
        var output = Environment.GetEnvironmentVariable("TOPEND_QA_EXPORT_DIR");
        if (!string.IsNullOrEmpty(output))
        {
            Directory.CreateDirectory(output);
            await File.WriteAllBytesAsync(Path.Combine(output, "Manager_Report_Synthetic_QA.pdf"), pdf);
            await File.WriteAllBytesAsync(Path.Combine(output, "Manager_Report_Synthetic_QA.csv"), ManagerReportExport.Csv(report));
        }
    }

    [Theory]
    [InlineData("=SUM(1,2)")]
    [InlineData("   +command")]
    [InlineData("\tformula")]
    [InlineData("@formula")]
    public void CsvCells_AreEscapedAgainstSpreadsheetFormulas(string text) =>
        Assert.StartsWith("\"'", ManagerReportExport.CsvCell(text));

    [Fact]
    public async Task EmptyLibrary_ReportsZeroWithoutDivisionErrors()
    {
        await using var db = TestData.NewContext();
        var report = await new ManagerReportingService(db, new(new FixedClock())).BuildAsync();
        Assert.Equal(0, report.TotalItems); Assert.Equal(0, report.AvailabilityRate);
        Assert.Equal(0m, report.TotalFines); Assert.Empty(report.FineAudit);
    }

    [Fact]
    public async Task ImportQuantity_CreatesDistinctCopiesAndReuploadSkipsExistingIsbn()
    {
        await using var db = TestData.NewContext(); await TestData.SeedReports(db);
        var controller = new CsvImportController(db, new(new FixedClock()), NullLogger<CsvImportController>.Instance);
        const string csv = "Title,Author,ISBN,Category,Quantity,PublicationYear\nQuantity Test,Demo,NEW-QUANTITY,Fiction,2,2024\n";
        var first = Assert.IsType<CsvImportViewModel>(Assert.IsType<ViewResult>(await controller.Upload(Upload(csv), 2)).Model);
        Assert.True(first.Succeeded); Assert.Equal(2, first.ImportedCopies); Assert.Equal(1, first.ImportedRows);
        var copies = await db.Books.Where(book => book.Isbn == "NEW-QUANTITY").ToListAsync();
        Assert.Equal(2, copies.Count); Assert.Equal(2, copies.Select(book => book.LibraryCode).Distinct().Count());
        Assert.All(copies, book => { Assert.Equal(2, book.BranchId); Assert.Equal(2024, book.PublicationYear); });
        var again = Assert.IsType<CsvImportViewModel>(Assert.IsType<ViewResult>(await controller.Upload(Upload(csv), 2)).Model);
        Assert.Equal(0, again.ImportedCopies); Assert.Equal(1, again.SkippedRows);
        Assert.Contains("ISBN already exists", again.Rows.Single().Detail);
    }

    [Fact]
    public async Task InvalidRows_AreExplainedWithoutLosingValidRows()
    {
        await using var db = TestData.NewContext(); await TestData.SeedReports(db);
        var controller = new CsvImportController(db, new(new FixedClock()), NullLogger<CsvImportController>.Instance);
        const string csv = "Title,Author,ISBN,Category,Quantity,PublicationYear\nValid Import,Demo,NEW-VALID,Fiction,1,2024\nBad Quantity,Demo,NEW-BADQ,Fiction,0,2024\nBad Year,Demo,NEW-BADY,Fiction,1,0\n";
        var result = Assert.IsType<CsvImportViewModel>(Assert.IsType<ViewResult>(await controller.Upload(Upload(csv), 1)).Model);
        Assert.Equal(1, result.ImportedCopies); Assert.Equal(2, result.SkippedRows);
        Assert.Contains(result.Rows, row => row.RowNumber == 3 && row.Detail.Contains("Quantity"));
        Assert.Contains(result.Rows, row => row.RowNumber == 4 && row.Detail.Contains("PublicationYear"));
        Assert.False(await db.Books.AnyAsync(book => book.Isbn == "NEW-BADY"));
    }

    [Fact]
    public async Task MissingHeadersAndInactiveBranch_AreRejected()
    {
        await using var db = TestData.NewContext(); await TestData.SeedReports(db);
        var controller = new CsvImportController(db, new(new FixedClock()), NullLogger<CsvImportController>.Instance);
        var invalid = Assert.IsType<CsvImportViewModel>(Assert.IsType<ViewResult>(await controller.Upload(Upload("Title,Quantity\nExample,1\n"), 1)).Model);
        Assert.False(invalid.Succeeded); Assert.Contains("PublicationYear", invalid.Message);
        var inactive = Assert.IsType<CsvImportViewModel>(Assert.IsType<ViewResult>(await controller.Upload(Upload("Title\nExample"), 3)).Model);
        Assert.False(inactive.Succeeded); Assert.Contains("active library branch", inactive.Message);
    }

    [Fact]
    public async Task CatalogueApi_SharesAvailabilityAndSupportsBranchCategoryFilters()
    {
        await using var db = TestData.NewContext(); await TestData.SeedReports(db);
        var api = new CatalogueApiController(db);
        var response = Assert.IsType<OkObjectResult>(await api.Available(category: "Book", branchId: 1));
        var records = JsonSerializer.SerializeToElement(response.Value);
        Assert.Equal(3, records.GetArrayLength());
        Assert.All(records.EnumerateArray(), record => Assert.True(record.GetProperty("Available").GetBoolean()));
        Assert.DoesNotContain("Membership", records.ToString());
        Assert.IsType<BadRequestObjectResult>(await api.Available(category: "Unknown"));
        Assert.IsType<BadRequestObjectResult>(await api.Available(branchId: 3));
    }

    [Fact]
    public void AdministrativeAndFinanceActions_DeclareRequiredRoleAndCsrfProtection()
    {
        var importRole = Assert.Single(typeof(CsvImportController).GetCustomAttributes(typeof(AuthorizeAttribute), true).Cast<AuthorizeAttribute>());
        Assert.Equal("Admin", importRole.Roles);
        var reportRole = Assert.Single(typeof(ManagerController).GetCustomAttributes(typeof(AuthorizeAttribute), true).Cast<AuthorizeAttribute>());
        Assert.Equal("Manager,Admin", reportRole.Roles);
        Assert.NotEmpty(typeof(CsvImportController).GetMethod("Upload")!.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), true));
        Assert.NotEmpty(typeof(LoansController).GetMethod("PayFine")!.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), true));
    }

    private static IFormFile Upload(string csv)
    {
        var bytes = Encoding.UTF8.GetBytes(csv);
        return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "file", "test.csv");
    }
}
