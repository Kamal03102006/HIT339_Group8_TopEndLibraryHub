using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;
using TopEndLibraryHub.Controllers;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Tests;

public class NotificationFilterTests
{
    [Fact]
    public async Task FailedManualCheck_KeepsTheSelectedFiltersForRetry()
    {
        await using var db = TestData.NewContext();
        // Deliberately unavailable check services exercise the controller's failure
        // response: retrying must not silently switch to an unfiltered result list.
        var controller = new NotificationsController(db, null!, null!, NullLogger<NotificationsController>.Instance)
        { TempData = new TempDataDictionary(new DefaultHttpContext(), new MemoryTempData()) };
        var response = Assert.IsType<RedirectToActionResult>(await controller.CheckNow(
            kind: NotificationKind.HoldReady, channel: NotificationChannel.Email,
            search: " ty-9001 ", page: 2));
        Assert.Equal("Index", response.ActionName);
        Assert.Equal("TY-9001", response.RouteValues!["search"]);
        Assert.Equal(NotificationKind.HoldReady, response.RouteValues["kind"]);
        Assert.Equal(NotificationChannel.Email, response.RouteValues["channel"]);
        Assert.Equal(2, response.RouteValues["page"]);
        Assert.NotNull(controller.TempData["NotificationError"]);
    }

    private sealed class MemoryTempData : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context) => new Dictionary<string, object>();
        public void SaveTempData(HttpContext context, IDictionary<string, object> values) { }
    }
}
