using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopEndLibraryHub.Services;

namespace TopEndLibraryHub.Controllers;

[Authorize(Roles = "Manager,Admin")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class ManagerController : Controller
{
    private readonly ManagerReportingService _reports;

    public ManagerController(ManagerReportingService reports) => _reports = reports;

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default) =>
        View(await _reports.BuildAsync(cancellationToken));

    [HttpGet]
    public async Task<IActionResult> ExportCsv(CancellationToken cancellationToken = default)
    {
        var report = await _reports.BuildAsync(cancellationToken);
        return File(ManagerReportExport.Csv(report), "text/csv; charset=utf-8",
            $"TopEndLibraryHub_Manager_{report.GeneratedAtDarwin:yyyyMMdd_HHmmss}.csv");
    }

    [HttpGet]
    public async Task<IActionResult> ExportPdf(CancellationToken cancellationToken = default)
    {
        var report = await _reports.BuildAsync(cancellationToken);
        return File(ManagerReportExport.Pdf(report), "application/pdf",
            $"TopEndLibraryHub_Manager_{report.GeneratedAtDarwin:yyyyMMdd_HHmmss}.pdf");
    }
}
