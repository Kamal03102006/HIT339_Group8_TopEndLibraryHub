using System.Globalization;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Services;

public static class ManagerReportExport
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    public static IEnumerable<(string Label, string Value)> Summary(ManagerDashboardViewModel r)
    {
        yield return ("Generated Darwin ACST", r.GeneratedAtDarwin.ToString("yyyy-MM-dd HH:mm:ss", Invariant));
        yield return ("Total Items", r.TotalItems.ToString(Invariant));
        yield return ("Generally Available Items", r.AvailableItems.ToString(Invariant));
        yield return ("Reserved Available Items", r.ReservedItems.ToString(Invariant));
        yield return ("Items In Transit", r.InTransitItems.ToString(Invariant));
        yield return ("Borrowed Items", r.BorrowedItems.ToString(Invariant));
        yield return ("Damaged Items", r.DamagedItems.ToString(Invariant));
        yield return ("Destroyed Items", r.DestroyedItems.ToString(Invariant));
        yield return ("Total Loans", r.TotalLoans.ToString(Invariant));
        yield return ("Active Loans", r.ActiveLoans.ToString(Invariant));
        yield return ("Overdue Loans", r.OverdueLoans.ToString(Invariant));
        yield return ("Returned Loans", r.ReturnedLoans.ToString(Invariant));
        yield return ("Recorded Fines AUD", Money(r.TotalFines));
        yield return ("Recorded Paid Fines AUD", Money(r.PaidFines));
        yield return ("Recorded Unpaid Fines AUD", Money(r.OutstandingFines));
        yield return ("Estimated Current Late Charges AUD", Money(r.EstimatedCurrentLateCharges));
    }

    public static byte[] Csv(ManagerDashboardViewModel report)
    {
        var text = new StringBuilder();
        void Row(params string[] cells) => text.AppendLine(string.Join(",", cells.Select(CsvCell)));
        Row("Metric", "Value");
        foreach (var (label, value) in Summary(report)) Row(label, value);
        text.AppendLine();
        Row("Loan ID", "Item Code", "Membership", "Due Date Darwin", "Returned Darwin ACST",
            "Recorded Fine AUD", "Paid", "Paid At Darwin ACST");
        foreach (var row in report.FineAudit)
            Row(row.LoanId.ToString(Invariant), row.ItemCode, row.MembershipNumber,
                row.DueDate.ToString("yyyy-MM-dd", Invariant), Timestamp(row.ReturnedAtDarwin),
                Money(row.FineAmount), row.Paid ? "Yes" : "No", Timestamp(row.PaidAtDarwin));
        text.AppendLine();
        Row("Item Code", "Title", "Category", "Physical Branch", "Physical Status",
            "Generally Available", "Reserved", "In Transit");
        foreach (var row in report.InventoryAudit)
            Row(row.ItemCode, row.Name, row.ItemType, row.Branch, row.PhysicalStatus,
                row.GenerallyAvailable ? "Yes" : "No", row.Reserved ? "Yes" : "No",
                row.InTransit ? "Yes" : "No");
        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(text.ToString())).ToArray();
    }

    public static byte[] Pdf(ManagerDashboardViewModel report)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        return Document.Create(document => document.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape());
            page.Margin(30);
            page.DefaultTextStyle(style => style.FontSize(9));
            page.Header().Column(header =>
            {
                header.Item().Text("Top End Library Hub Manager Report").FontSize(19).Bold();
                header.Item().Text($"Generated {report.GeneratedAtDarwin:dd MMM yyyy, HH:mm:ss} Darwin (ACST) | Currency AUD");
            });
            page.Content().PaddingVertical(14).Column(content =>
            {
                content.Spacing(12);
                content.Item().Text("Borrowing and inventory summary").FontSize(13).Bold();
                content.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns => { columns.RelativeColumn(2); columns.RelativeColumn(); });
                    table.Header(header => { Cell(header.Cell(), "Metric", true); Cell(header.Cell(), "Value", true); });
                    foreach (var (label, value) in Summary(report))
                    { Cell(table.Cell(), label); Cell(table.Cell(), value); }
                });
                content.Item().EnsureSpace(130).Text("Recorded fine audit").FontSize(13).Bold();
                content.Item().Text("Current late-charge estimates are not included in recorded fine or payment totals. Paid means recorded as paid in the library system.");
                if (report.FineAudit.Count == 0) content.Item().Text("No recorded non-zero fines.");
                else content.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(.5f); columns.RelativeColumn(); columns.RelativeColumn();
                        columns.RelativeColumn(); columns.RelativeColumn(.8f); columns.RelativeColumn(.5f);
                        columns.RelativeColumn(1.5f);
                    });
                    table.Header(header =>
                    {
                        foreach (var label in new[] { "Loan", "Item", "Membership", "Due date", "Fine AUD", "Paid", "Paid at ACST" })
                            Cell(header.Cell(), label, true);
                    });
                    foreach (var row in report.FineAudit.Take(200))
                    {
                        Cell(table.Cell(), row.LoanId.ToString(Invariant)); Cell(table.Cell(), row.ItemCode);
                        Cell(table.Cell(), row.MembershipNumber); Cell(table.Cell(), row.DueDate.ToString("yyyy-MM-dd", Invariant));
                        Cell(table.Cell(), Money(row.FineAmount)); Cell(table.Cell(), row.Paid ? "Yes" : "No");
                        Cell(table.Cell(), Timestamp(row.PaidAtDarwin));
                    }
                });
                if (report.FineAudit.Count > 200) content.Item().Text("First 200 fine records shown. The CSV export contains the complete audit.");
                content.Item().Text("Inventory health and availability").FontSize(13).Bold();
                content.Item().Text("General availability excludes active loans, transfers, reserved copies and inactive branches. Physical Available status alone does not mean a copy can be borrowed.");
                content.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(); columns.RelativeColumn(2); columns.RelativeColumn();
                        columns.RelativeColumn(1.5f); columns.RelativeColumn(); columns.RelativeColumn(.8f);
                        columns.RelativeColumn(.8f); columns.RelativeColumn(.8f);
                    });
                    table.Header(header =>
                    {
                        foreach (var label in new[] { "Code", "Title", "Category", "Branch", "Physical status", "Available", "Reserved", "In transit" })
                            Cell(header.Cell(), label, true);
                    });
                    foreach (var row in report.InventoryAudit.Take(200))
                    {
                        Cell(table.Cell(), row.ItemCode); Cell(table.Cell(), row.Name); Cell(table.Cell(), row.ItemType);
                        Cell(table.Cell(), row.Branch); Cell(table.Cell(), row.PhysicalStatus);
                        Cell(table.Cell(), row.GenerallyAvailable ? "Yes" : "No"); Cell(table.Cell(), row.Reserved ? "Yes" : "No");
                        Cell(table.Cell(), row.InTransit ? "Yes" : "No");
                    }
                });
                if (report.InventoryAudit.Count > 200) content.Item().Text("First 200 physical copies shown. The CSV export contains the full inventory.");
            });
            page.Footer().AlignRight().Text(text =>
            { text.Span($"Top End Library Hub | {report.GeneratedAtDarwin:yyyy-MM-dd HH:mm} ACST | "); text.CurrentPageNumber(); text.Span(" / "); text.TotalPages(); });
        })).GeneratePdf();
    }

    private static void Cell(IContainer container, string value, bool heading = false)
    {
        var cell = container.BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4);
        if (heading) cell.Background(Colors.Grey.Lighten3).Text(value).Bold();
        else cell.Text(value);
    }

    public static string CsvCell(string? value)
    {
        value ??= string.Empty;
        var first = value.TrimStart();
        if (value.Length > 0 && (first.StartsWith('=') || first.StartsWith('+') ||
            first.StartsWith('-') || first.StartsWith('@') || value[0] is '\t' or '\r' or '\n'))
            value = "'" + value;
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

    private static string Money(decimal value) => value.ToString("0.00", Invariant);
    private static string Timestamp(DateTime? value) => value?.ToString("yyyy-MM-dd HH:mm:ss", Invariant) ?? string.Empty;
}
