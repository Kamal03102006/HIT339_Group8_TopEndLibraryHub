using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace TopEndLibraryHub.Middleware;

public class KioskSessionMiddleware
{
    private readonly RequestDelegate _next;
    public KioskSessionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, SignInManager<IdentityUser> signIn,
        TimeProvider clock)
    {
        var action = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
        var isPrivateKiosk = action?.ControllerName == "Kiosk" &&
            action.ActionName is "Account" or "Scan" or "Checkout" or "KeepAlive" or "Hold" or "Index";
        if (!isPrivateKiosk || context.User.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        context.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
        context.Response.Headers.Pragma = "no-cache";
        await context.Session.LoadAsync(context.RequestAborted);
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var now = clock.GetUtcNow().ToUnixTimeMilliseconds();
        var sameUser = context.Session.GetString("KioskUserId") == userId;
        var hasLastActivity = long.TryParse(context.Session.GetString("KioskLastActivity"),
            NumberStyles.Integer, CultureInfo.InvariantCulture, out var lastActivity);
        if (sameUser && hasLastActivity && now - lastActivity >= 120_000)
        {
            await signIn.SignOutAsync();
            context.Session.Clear();
            if (action!.ActionName == "KeepAlive")
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Kiosk session expired." });
            }
            else context.Response.Redirect("/Kiosk?expired=true");
            return;
        }

        context.Session.SetString("KioskUserId", userId ?? string.Empty);
        context.Session.SetString("KioskLastActivity", now.ToString(CultureInfo.InvariantCulture));
        await _next(context);
    }
}
