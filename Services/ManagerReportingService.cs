using System.Data;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Services;

public class ManagerReportingService
{
    private readonly ApplicationDbContext _db;
    private readonly LibraryClock _clock;

    public ManagerReportingService(ApplicationDbContext db, LibraryClock clock)
    {
        _db = db;
        _clock = clock;
    }

    // The dashboard and both exports use this definition of available stock and
    // recorded fines. The SQL Server transaction keeps each report consistent
    // while its source data is read; it ends before any PDF rendering starts.
    public async Task<ManagerDashboardViewModel> BuildAsync(
        CancellationToken cancellationToken = default)
    {
        await using var transaction = _db.Database.IsRelational()
            ? await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken)
            : null;
        var generated = _clock.ToDarwinTime(_clock.UtcNow);
        var today = generated.Date;
        var items = await _db.Items.AsNoTracking().Include(item => item.Branch)
            .OrderBy(item => item.LibraryCode).ToListAsync(cancellationToken);
        var availableIds = (await CirculationRules.GenerallyAvailable(_db)
            .Select(item => item.Id).ToListAsync(cancellationToken)).ToHashSet();
        var reservedIds = (await CirculationRules.ReservedItemIds(_db)
            .ToListAsync(cancellationToken)).ToHashSet();
        var transitIds = (await _db.ItemTransfers.AsNoTracking()
            .Where(transfer => transfer.Status == TransferStatus.InTransit)
            .Select(transfer => transfer.ItemId).ToListAsync(cancellationToken)).ToHashSet();
        var loans = await _db.Loans.AsNoTracking().Include(loan => loan.Item)
            .Include(loan => loan.Borrower).ToListAsync(cancellationToken);

        var result = new ManagerDashboardViewModel
        {
            GeneratedAtDarwin = generated,
            TotalItems = items.Count,
            AvailableItems = availableIds.Count,
            ReservedItems = items.Count(item => item.Status == ItemStatus.Available &&
                !transitIds.Contains(item.Id) && reservedIds.Contains(item.Id)),
            InTransitItems = items.Count(item => item.Status == ItemStatus.InTransit || transitIds.Contains(item.Id)),
            BorrowedItems = items.Count(item => item.Status == ItemStatus.Borrowed),
            DamagedItems = items.Count(item => item.Status == ItemStatus.Damaged),
            DestroyedItems = items.Count(item => item.Status == ItemStatus.Destroyed),
            TotalLoans = loans.Count,
            ActiveLoans = loans.Count(loan => loan.ReturnedDate is null),
            ReturnedLoans = loans.Count(loan => loan.ReturnedDate is not null),
            OverdueLoans = loans.Count(loan => loan.ReturnedDate is null && loan.DueDate.Date < today),
            TotalFines = loans.Sum(loan => loan.FineAmount),
            PaidFines = loans.Where(loan => loan.FinePaid).Sum(loan => loan.FineAmount),
            OutstandingFines = loans.Where(loan => !loan.FinePaid).Sum(loan => loan.FineAmount),
            EstimatedCurrentLateCharges = loans.Where(loan => loan.ReturnedDate is null)
                .Sum(loan => FinePolicy.Calculate(loan.DueDate, today))
        };
        result.AvailabilityRate = result.TotalItems == 0 ? 0 :
            100d * result.AvailableItems / result.TotalItems;
        result.ItemsByType = items.GroupBy(ItemType).OrderBy(group => group.Key)
            .Select(group => new ItemTypeStatisticViewModel
            {
                ItemType = group.Key,
                TotalItems = group.Count(),
                AvailableItems = group.Count(item => availableIds.Contains(item.Id))
            }).ToList();
        result.PopularItems = loans.GroupBy(loan => loan.ItemId)
            .OrderByDescending(group => group.Count()).ThenBy(group => group.First().Item.Name)
            .Take(5).Select(group => new PopularItemViewModel
            {
                ItemId = group.Key,
                LibraryCode = group.First().Item.LibraryCode,
                Name = group.First().Item.Name,
                LoanCount = group.Count()
            }).ToList();
        result.RecentLoans = loans.OrderByDescending(loan => loan.BorrowedDate)
            .ThenByDescending(loan => loan.Id).Take(5).Select(loan => new RecentLoanViewModel
            {
                LoanId = loan.Id, LibraryCode = loan.Item.LibraryCode,
                ItemName = loan.Item.Name, BorrowerName = loan.Borrower.FullName,
                BorrowedDate = _clock.ToDarwinTime(loan.BorrowedDate),
                DueDate = loan.DueDate,
                ReturnedDate = loan.ReturnedDate.HasValue ? _clock.ToDarwinTime(loan.ReturnedDate.Value) : null,
                FineAmount = loan.FineAmount, FinePaid = loan.FinePaid
            }).ToList();
        result.FineAudit = loans.Where(loan => loan.FineAmount > 0).OrderBy(loan => loan.Id)
            .Select(loan => new FineAuditRow
            {
                LoanId = loan.Id, ItemCode = loan.Item.LibraryCode,
                MembershipNumber = loan.Borrower.MembershipNumber,
                DueDate = loan.DueDate.Date,
                ReturnedAtDarwin = loan.ReturnedDate.HasValue ? _clock.ToDarwinTime(loan.ReturnedDate.Value) : null,
                FineAmount = loan.FineAmount, Paid = loan.FinePaid,
                PaidAtDarwin = loan.FinePaidDate.HasValue ? _clock.ToDarwinTime(loan.FinePaidDate.Value) : null
            }).ToList();
        result.InventoryAudit = items.Select(item => new InventoryAuditRow
        {
            ItemCode = item.LibraryCode, Name = item.Name, ItemType = ItemType(item),
            Branch = item.Branch?.Name ?? "Unknown branch", PhysicalStatus = item.Status.ToString(),
            GenerallyAvailable = availableIds.Contains(item.Id),
            Reserved = reservedIds.Contains(item.Id),
            InTransit = item.Status == ItemStatus.InTransit || transitIds.Contains(item.Id)
        }).ToList();

        if (transaction is not null) await transaction.CommitAsync(cancellationToken);
        return result;
    }

    private static string ItemType(Item item) => item switch
    {
        Book => "Books", Music => "Music", Toy => "Toys", _ => "Other"
    };
}
