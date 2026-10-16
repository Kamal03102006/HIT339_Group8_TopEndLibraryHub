using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Controllers;
using TopEndLibraryHub.Services;

namespace TopEndLibraryHub.Tests;

public class PaymentActionTests
{
    [Fact]
    public async Task PayingRecordedFine_MovesOnlyThatAmountFromUnpaidToPaid_AndRepeatIsIdempotent()
    {
        await using var db = TestData.NewContext(); await TestData.SeedReports(db);
        var time = new FixedClock(); var clock = new LibraryClock(time);
        var controller = new LoansController(db, null!, clock)
        { TempData = new TempDataDictionary(new DefaultHttpContext(), new MemoryTempData()) };
        var reports = new ManagerReportingService(db, clock);
        var before = await reports.BuildAsync();
        Assert.Equal(6m, before.OutstandingFines); Assert.Equal(3m, before.PaidFines);
        Assert.IsType<RedirectToActionResult>(await controller.PayFine(3));
        var after = await reports.BuildAsync();
        Assert.Equal(0m, after.OutstandingFines); Assert.Equal(9m, after.PaidFines);
        Assert.Equal(before.TotalFines, after.TotalFines);
        var paidAt = (await db.Loans.SingleAsync(loan => loan.Id == 3)).FinePaidDate;
        Assert.Equal(clock.UtcNow, paidAt);
        time.Now = time.Now.AddHours(1);
        await controller.PayFine(3);
        Assert.Equal(paidAt, (await db.Loans.SingleAsync(loan => loan.Id == 3)).FinePaidDate);
        Assert.Equal(9m, (await reports.BuildAsync()).PaidFines);
    }

    private sealed class MemoryTempData : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context) => new Dictionary<string, object>();
        public void SaveTempData(HttpContext context, IDictionary<string, object> values) { }
    }
}
