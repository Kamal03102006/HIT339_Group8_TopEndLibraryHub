using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;

namespace TopEndLibraryHub.Tests;

public sealed class SqlServerFactAttribute : FactAttribute
{
    public SqlServerFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TOPEND_TEST_SQLSERVER")))
            Skip = "Set TOPEND_TEST_SQLSERVER to run isolated SQL Server migration and circulation checks.";
    }
}

public class SqlServerCirculationTests
{
    [SqlServerFact]
    public async Task ReadyOwner_CanCollectWhileAnotherMemberWaits()
    {
        await using var test = await SqlTestLibrary.CreateAsync();
        var db = test.Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var now = test.Clock.UtcNow;
        db.Holds.AddRange(new Hold { ItemId = test.ItemId, BorrowerId = test.FirstBorrower,
            PickupBranchId = test.BranchId, Status = HoldStatus.ReadyForPickup,
            CreatedAtUtc = now.AddHours(-2), AllocatedAtUtc = now.AddHours(-1), ReadyForPickupAtUtc = now.AddHours(-1) },
            new Hold { ItemId = test.ItemId, BorrowerId = test.NextBorrower,
                PickupBranchId = test.BranchId, Status = HoldStatus.Waiting, CreatedAtUtc = now });
        await db.SaveChangesAsync();
        var result = await test.Scope.ServiceProvider.GetRequiredService<CirculationService>()
            .CheckoutAsync("BK-9801", test.FirstBorrower, test.BranchId, test.Clock.Today.AddDays(14));
        Assert.True(result.Succeeded, result.Message);
        Assert.Equal(HoldStatus.Fulfilled, (await db.Holds.SingleAsync(hold => hold.BorrowerId == test.FirstBorrower)).Status);
        Assert.Equal(HoldStatus.Waiting, (await db.Holds.SingleAsync(hold => hold.BorrowerId == test.NextBorrower)).Status);
        Assert.Equal(1, await db.Loans.CountAsync(loan => loan.ReturnedDate == null));
    }

    [SqlServerFact]
    public async Task TwoSimultaneousCheckouts_CreateOneLoan()
    {
        await using var test = await SqlTestLibrary.CreateAsync();
        async Task<CirculationResult> Checkout()
        {
            await using var scope = test.Provider.CreateAsyncScope();
            return await scope.ServiceProvider.GetRequiredService<CirculationService>()
                .CheckoutAsync("BK-9801", test.FirstBorrower, test.BranchId, test.Clock.Today.AddDays(14));
        }
        var results = await Task.WhenAll(Checkout(), Checkout());
        Assert.Single(results, result => result.Succeeded);
        var db = test.Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Assert.Equal(1, await db.Loans.CountAsync(loan => loan.ItemId == test.ItemId && loan.ReturnedDate == null));
    }

    [SqlServerFact]
    public async Task LateReturn_RecordsFineAndPromotesTheNextHold()
    {
        await using var test = await SqlTestLibrary.CreateAsync();
        var db = test.Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var circulation = test.Scope.ServiceProvider.GetRequiredService<CirculationService>();
        var checkout = await circulation.CheckoutAsync("BK-9801", test.FirstBorrower,
            test.BranchId, test.Clock.Today.AddDays(14));
        Assert.True(checkout.Succeeded, checkout.Message);
        db.Holds.Add(new Hold { ItemId = test.ItemId, BorrowerId = test.NextBorrower,
            PickupBranchId = test.BranchId, Status = HoldStatus.Waiting, CreatedAtUtc = test.Clock.UtcNow });
        await db.SaveChangesAsync();
        test.Time.Now = test.Time.Now.AddDays(18);
        var returned = await circulation.ReturnAsync("BK-9801", test.Clock.Today, false, false,
            test.BranchId, test.DeskId);
        Assert.True(returned.Succeeded, returned.Message);
        var loan = await db.Loans.SingleAsync();
        Assert.Equal(6m, loan.FineAmount); Assert.False(loan.FinePaid); Assert.Null(loan.FinePaidDate);
        Assert.Equal(HoldStatus.ReadyForPickup, (await db.Holds.SingleAsync()).Status);
        var report = await test.Scope.ServiceProvider.GetRequiredService<ManagerReportingService>().BuildAsync();
        Assert.Equal(6m, report.OutstandingFines); Assert.Equal(0m, report.PaidFines);
        Assert.Equal(0, report.AvailableItems); Assert.Equal(1, report.ReservedItems);
    }
}

internal sealed class SqlTestLibrary : IAsyncDisposable
{
    public ServiceProvider Provider { get; private init; } = null!;
    public AsyncServiceScope Scope { get; private set; }
    public FixedClock Time { get; private init; } = new();
    public LibraryClock Clock => Scope.ServiceProvider.GetRequiredService<LibraryClock>();
    public int BranchId { get; private set; }
    public int DeskId { get; private set; }
    public int ItemId { get; private set; }
    public int FirstBorrower { get; private set; }
    public int NextBorrower { get; private set; }
    private string ConnectionString { get; init; } = string.Empty;

    public static async Task<SqlTestLibrary> CreateAsync()
    {
        var supplied = Environment.GetEnvironmentVariable("TOPEND_TEST_SQLSERVER")
            ?? throw new InvalidOperationException("SQL test connection not configured.");
        // Ignore any supplied database name. Only this newly generated test database
        // can be created or deleted; the application's demonstration database is untouched.
        var builder = new SqlConnectionStringBuilder(supplied)
        { InitialCatalog = "TopEndLibraryHub_Tests_" + Guid.NewGuid().ToString("N") };
        var services = new ServiceCollection(); var time = new FixedClock();
        services.AddLogging(); services.AddSingleton<TimeProvider>(time); services.AddSingleton<LibraryClock>();
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.ConnectionString));
        services.AddScoped<NotificationService>(); services.AddScoped<HoldWorkflowService>();
        services.AddScoped<CirculationService>(); services.AddScoped<ManagerReportingService>();
        var test = new SqlTestLibrary { Provider = services.BuildServiceProvider(), Time = time,
            ConnectionString = builder.ConnectionString };
        test.Scope = test.Provider.CreateAsyncScope();
        try
        {
            var db = test.Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();
            var branch = new Branch { BranchCode = "TEST", Name = "SQL test branch", Address = "Fictional", IsActive = true };
            db.Branches.Add(branch); await db.SaveChangesAsync(); test.BranchId = branch.Id;
            var desk = new ReceptionDesk { BranchId = branch.Id, DeskCode = "TEST-D01", Name = "SQL test desk", IsActive = true };
            var first = new Borrower { MembershipNumber = "MBR-9801", FullName = "SQL test first", Email = "sql-test-first@example.com", Phone = "0400000001", Address = "Fictional", IsActive = true };
            var next = new Borrower { MembershipNumber = "MBR-9802", FullName = "SQL test next", Email = "sql-test-next@example.com", Phone = "0400000002", Address = "Fictional", IsActive = true };
            var item = TestData.Book(0, branch.Id); item.LibraryCode = "BK-9801";
            db.ReceptionDesks.Add(desk); db.Borrowers.AddRange(first, next); db.Books.Add(item);
            await db.SaveChangesAsync(); test.DeskId = desk.Id; test.FirstBorrower = first.Id;
            test.NextBorrower = next.Id; test.ItemId = item.Id; db.ChangeTracker.Clear();
            return test;
        }
        catch { await test.DisposeAsync(); throw; }
    }

    public async ValueTask DisposeAsync()
    {
        await Scope.DisposeAsync();
        await using var cleanup = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(ConnectionString).Options);
        if (!new SqlConnectionStringBuilder(ConnectionString).InitialCatalog.StartsWith("TopEndLibraryHub_Tests_", StringComparison.Ordinal))
            throw new InvalidOperationException("Refusing cleanup outside the isolated test database.");
        try { await cleanup.Database.EnsureDeletedAsync(); }
        finally { await Provider.DisposeAsync(); }
    }
}
