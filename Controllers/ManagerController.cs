using System.Globalization;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.UtcNow.Date;

            var viewModel = new ManagerDashboardViewModel
            {
                // Item statistics
                TotalItems = await _context.Items.CountAsync(),
                AvailableItems = await _context.Items.CountAsync(
                    item => item.Status == ItemStatus.Available),
                BorrowedItems = await _context.Items.CountAsync(
                    item => item.Status == ItemStatus.Borrowed),
                DamagedItems = await _context.Items.CountAsync(
                    item => item.Status == ItemStatus.Damaged),
                DestroyedItems = await _context.Items.CountAsync(
                    item => item.Status == ItemStatus.Destroyed),

                // Loan statistics
                TotalLoans = await _context.Loans.CountAsync(),
                ActiveLoans = await _context.Loans.CountAsync(
                    loan => loan.ReturnedDate == null),
                OverdueLoans = await _context.Loans.CountAsync(
                    loan => loan.ReturnedDate == null &&
                            loan.DueDate < today),
                ReturnedLoans = await _context.Loans.CountAsync(
                    loan => loan.ReturnedDate != null),

                // Fine statistics
                TotalFines = await _context.Loans
                    .SumAsync(loan => (decimal?)loan.FineAmount) ?? 0m,

                PaidFines = await _context.Loans
                    .Where(loan => loan.FinePaid)
                    .SumAsync(loan => (decimal?)loan.FineAmount) ?? 0m,

                OutstandingFines = await _context.Loans
                    .Where(loan => loan.FineAmount > 0 && !loan.FinePaid)
                    .SumAsync(loan => (decimal?)loan.FineAmount) ?? 0m
            };

            viewModel.AvailabilityRate = viewModel.TotalItems == 0
                ? 0
                : (double)viewModel.AvailableItems /
                  viewModel.TotalItems * 100;

            viewModel.ItemsByType =
            [
                new ItemTypeStatisticViewModel
                {
                    ItemType = "Books",
                    TotalItems = await _context.Books.CountAsync(),
                    AvailableItems = await _context.Books.CountAsync(
                        item => item.Status == ItemStatus.Available)
                },
                new ItemTypeStatisticViewModel
                {
                    ItemType = "Music",
                    TotalItems = await _context.MusicItems.CountAsync(),
                    AvailableItems = await _context.MusicItems.CountAsync(
                        item => item.Status == ItemStatus.Available)
                },
                new ItemTypeStatisticViewModel
                {
                    ItemType = "Toys",
                    TotalItems = await _context.Toys.CountAsync(),
                    AvailableItems = await _context.Toys.CountAsync(
                        item => item.Status == ItemStatus.Available)
                }
            ];

            viewModel.PopularItems = await _context.Loans
                .AsNoTracking()
                .GroupBy(loan => new
                {
                    loan.ItemId,
                    loan.Item.LibraryCode,
                    loan.Item.Name
                })
                .Select(group => new PopularItemViewModel
                {
                    ItemId = group.Key.ItemId,
                    LibraryCode = group.Key.LibraryCode,
                    Name = group.Key.Name,
                    LoanCount = group.Count()
                })
                .OrderByDescending(item => item.LoanCount)
                .ThenBy(item => item.Name)
                .Take(5)
                .ToListAsync();

            viewModel.RecentLoans = await _context.Loans
                .AsNoTracking()
                .OrderByDescending(loan => loan.BorrowedDate)
                .ThenByDescending(loan => loan.Id)
                .Take(5)
                .Select(loan => new RecentLoanViewModel
                {
                    LoanId = loan.Id,
                    LibraryCode = loan.Item.LibraryCode,
                    ItemName = loan.Item.Name,
                    BorrowerName = loan.Borrower.FullName,
                    BorrowedDate = loan.BorrowedDate,
                    DueDate = loan.DueDate,
                    ReturnedDate = loan.ReturnedDate,
                    FineAmount = loan.FineAmount,
                    FinePaid = loan.FinePaid
                })
                .ToListAsync();

            return View(viewModel);
        }
        
[HttpGet]
public async Task<IActionResult> ExportCsv()
{
    var today = DateTime.UtcNow.Date;

    var totalItems = await _context.Items.CountAsync();

    var availableItems = await _context.Items
        .CountAsync(i => i.Status == ItemStatus.Available);

    var borrowedItems = await _context.Items
        .CountAsync(i => i.Status == ItemStatus.Borrowed);

    var damagedItems = await _context.Items
        .CountAsync(i => i.Status == ItemStatus.Damaged);

    var destroyedItems = await _context.Items
        .CountAsync(i => i.Status == ItemStatus.Destroyed);

    var totalLoans = await _context.Loans.CountAsync();

    var activeLoans = await _context.Loans
        .CountAsync(l => l.ReturnedDate == null);

    var overdueLoans = await _context.Loans
        .CountAsync(l =>
            l.ReturnedDate == null && l.DueDate < today);

    var returnedLoans = await _context.Loans
        .CountAsync(l => l.ReturnedDate != null);

    var totalFines = await _context.Loans
        .SumAsync(l => (decimal?)l.FineAmount) ?? 0m;

    var paidFines = await _context.Loans
        .Where(l => l.FinePaid)
        .SumAsync(l => (decimal?)l.FineAmount) ?? 0m;

    var outstandingFines = await _context.Loans
        .Where(l => l.FineAmount > 0 && !l.FinePaid)
        .SumAsync(l => (decimal?)l.FineAmount) ?? 0m;

    var csv = new StringBuilder();
    csv.AppendLine("Report,Value");

    void AddRow(string name, string value)
    {
        var safeName = name.Replace("\"", "\"\"");
        var safeValue = value.Replace("\"", "\"\"");
        csv.AppendLine($"\"{safeName}\",\"{safeValue}\"");
    }

    AddRow("Total Items", totalItems.ToString());
    AddRow("Available Items", availableItems.ToString());
    AddRow("Borrowed Items", borrowedItems.ToString());
    AddRow("Damaged Items", damagedItems.ToString());
    AddRow("Destroyed Items", destroyedItems.ToString());
    AddRow("Total Loans", totalLoans.ToString());
    AddRow("Active Loans", activeLoans.ToString());
    AddRow("Overdue Loans", overdueLoans.ToString());
    AddRow("Returned Loans", returnedLoans.ToString());

    AddRow("Total Fines",
        totalFines.ToString(CultureInfo.InvariantCulture));
    AddRow("Paid Fines",
        paidFines.ToString(CultureInfo.InvariantCulture));
    AddRow("Outstanding Fines",
        outstandingFines.ToString(CultureInfo.InvariantCulture));

    var bytes = new UTF8Encoding(true)
        .GetBytes(csv.ToString());

    return File(
        bytes,
        "text/csv",
        $"LibraryManagerReport_{DateTime.UtcNow:yyyyMMdd}.csv");
}

[HttpGet]
public async Task<IActionResult> ExportPdf()
{
    // Use the same statistics as the manager dashboard.
    var totalItems = await _context.Items.CountAsync();
    var available = await _context.Items.CountAsync(
        i => i.Status == ItemStatus.Available);
    var borrowed = await _context.Items.CountAsync(
        i => i.Status == ItemStatus.Borrowed);
    var damaged = await _context.Items.CountAsync(
        i => i.Status == ItemStatus.Damaged);
    var destroyed = await _context.Items.CountAsync(
        i => i.Status == ItemStatus.Destroyed);

    var today = DateTime.UtcNow.Date;

    var totalLoans = await _context.Loans.CountAsync();
    var activeLoans = await _context.Loans.CountAsync(
        l => l.ReturnedDate == null);
    var overdueLoans = await _context.Loans.CountAsync(
        l => l.ReturnedDate == null && l.DueDate < today);
    var returnedLoans = await _context.Loans.CountAsync(
        l => l.ReturnedDate != null);

    var totalFines = await _context.Loans
        .SumAsync(l => (decimal?)l.FineAmount) ?? 0m;
    var paidFines = await _context.Loans
        .Where(l => l.FinePaid)
        .SumAsync(l => (decimal?)l.FineAmount) ?? 0m;
    var outstandingFines = await _context.Loans
        .Where(l => l.FineAmount > 0 && !l.FinePaid)
        .SumAsync(l => (decimal?)l.FineAmount) ?? 0m;

    // Configure QuestPDF community licensing.
    // Confirm the project qualifies for this licence.
    QuestPDF.Settings.License = LicenseType.Community;

    var rows = new (string Label, string Value)[]
    {
        ("Total Items", totalItems.ToString()),
        ("Available Items", available.ToString()),
        ("Borrowed Items", borrowed.ToString()),
        ("Damaged Items", damaged.ToString()),
        ("Destroyed Items", destroyed.ToString()),
        ("Total Loans", totalLoans.ToString()),
        ("Active Loans", activeLoans.ToString()),
        ("Overdue Loans", overdueLoans.ToString()),
        ("Returned Loans", returnedLoans.ToString()),
        ("Total Fines", totalFines.ToString("C")),
        ("Paid Fines", paidFines.ToString("C")),
        ("Outstanding Fines", outstandingFines.ToString("C"))
    };

    var pdf = Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(35);
            page.DefaultTextStyle(x => x.FontSize(11));

            page.Header().Column(column =>
            {
                column.Item().Text("Top End Library Hub")
                    .Bold().FontSize(20);

                column.Item().Text("Manager Summary Report")
                    .FontSize(15);

                column.Item().Text(
                    $"Generated: {DateTime.UtcNow:dd MMM yyyy HH:mm} UTC")
                    .FontSize(9);
            });

            page.Content().PaddingVertical(20).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                });

                table.Header(header =>
                {
                    header.Cell().Padding(8)
                        .Text("Report").Bold();

                    header.Cell().Padding(8)
                        .Text("Value").Bold();
                });

                foreach (var row in rows)
                {
                    table.Cell().BorderBottom(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .Padding(8).Text(row.Label);

                    table.Cell().BorderBottom(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .Padding(8).Text(row.Value);
                }
            });

            page.Footer().AlignCenter()
                .Text("Top End Library Hub - Manager Report");
        });
    }).GeneratePdf();

    return File(
        pdf,
        "application/pdf",
        $"LibraryManagerReport_{DateTime.UtcNow:yyyyMMdd}.pdf");
}


    }
}