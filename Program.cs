using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure the SQL Server database.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure authentication, secure passwords and staff roles.
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

// Enable MVC controllers and views.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply migrations and create demonstration staff accounts.
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enable routing.
app.UseRouting();

// Enable authentication and authorization.
app.UseAuthentication();
app.UseAuthorization();

// Protect the catalogue API using an API key.
app.UseMiddleware<CatalogueApiKeyMiddleware>();

// Enable static files.
app.MapStaticAssets();

// Configure default MVC routing.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Enable ASP.NET Core Identity pages.
app.MapRazorPages()
    .WithStaticAssets();

// Start the application.
app.Run();