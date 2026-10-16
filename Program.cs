using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Services;
using TopEndLibraryHub.Middleware;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(10);
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.LoginPath = "/Identity/Account/Login";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Events.OnSigningIn = context =>
    {
        var staff = context.Principal?.IsInRole("Admin") == true ||
            context.Principal?.IsInRole("Reception") == true ||
            context.Principal?.IsInRole("Manager") == true;
        if (!staff)
        {
            context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(2);
            context.Properties.IsPersistent = false;
        }
        return Task.CompletedTask;
    };
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<HoldQueueService>();
builder.Services.AddScoped<HoldWorkflowService>();
builder.Services.AddScoped<HoldPlacementService>();
builder.Services.AddScoped<CirculationService>();
builder.Services.AddScoped<ManagerReportingService>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<LibraryClock>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddHostedService<NotificationWorker>();
builder.Services.Configure<Microsoft.AspNetCore.Builder.RequestLocalizationOptions>(options =>
    options.SetDefaultCulture("en-AU").AddSupportedCultures("en-AU").AddSupportedUICultures("en-AU"));
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
var app = builder.Build();

try
{
    await DbInitializer.InitialiseAsync(app.Services);
}
catch (Exception exception)
{
    var logger = app.Services
        .GetRequiredService<ILogger<Program>>();

    logger.LogCritical(
        exception,
        "The database could not be initialised.");

    throw;
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseMiddleware<KioskSessionMiddleware>();
app.UseAuthorization();

app.UseMiddleware<CatalogueApiKeyMiddleware>();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();
