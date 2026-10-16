using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;

namespace TopEndLibraryHub.Tests;

public sealed class FixedClock : TimeProvider
{
    public DateTimeOffset Now { get; set; } = DateTimeOffset.Parse("2026-10-15T15:00:00Z");
    public override DateTimeOffset GetUtcNow() => Now;
}

public static class TestData
{
    public static ApplicationDbContext NewContext() => new(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    public static Book Book(int id, int branchId = 1, ItemStatus status = ItemStatus.Available) => new()
    {
        Id = id, BranchId = branchId, Status = status, LibraryCode = $"BK-{id:D4}",
        Name = $"Test Book {id}", Description = "Fictional book for automated testing",
        Author = "Test Author", BookGenre = "Fiction", PublicationYear = 2024,
        Isbn = $"TEST-{id:D4}"
    };

    public static async Task SeedReports(ApplicationDbContext db)
    {
        db.Branches.AddRange(new Branch { Id = 1, BranchCode = "DAR", Name = "Darwin", Address = "Demo", IsActive = true },
            new Branch { Id = 2, BranchCode = "PAL", Name = "Palmerston", Address = "Demo", IsActive = true },
            new Branch { Id = 3, BranchCode = "OFF", Name = "Inactive", Address = "Demo", IsActive = false });
        db.Borrowers.AddRange(new Borrower { Id = 1, MembershipNumber = "MBR-1001", FullName = "Active Test", Email = "active@example.com", Phone = "0400000001", Address = "Demo", IsActive = true },
            new Borrower { Id = 2, MembershipNumber = "MBR-1002", FullName = "Inactive Test", Email = "inactive@example.com", Phone = "0400000002", Address = "Demo", IsActive = false },
            new Borrower { Id = 3, MembershipNumber = "MBR-1003", FullName = "Next Test", Email = "next@example.com", Phone = "0400000003", Address = "Demo", IsActive = true });
        for (var id = 1; id <= 13; id++)
            db.Books.Add(Book(id, id == 7 ? 3 : 1, id switch
            { 4 => ItemStatus.InTransit, 5 => ItemStatus.Damaged, 6 => ItemStatus.Borrowed, 13 => ItemStatus.Destroyed, _ => ItemStatus.Available }));
        var created = DateTime.Parse("2026-10-14T00:00:00Z").ToUniversalTime();
        db.Holds.AddRange(
            new Hold { Id = 1, ItemId = 2, BorrowerId = 1, PickupBranchId = 1, Status = HoldStatus.ReadyForPickup, CreatedAtUtc = created, ReadyForPickupAtUtc = created },
            new Hold { Id = 2, ItemId = 2, BorrowerId = 3, PickupBranchId = 1, Status = HoldStatus.Waiting, CreatedAtUtc = created.AddMinutes(1) },
            new Hold { Id = 3, ItemId = 3, BorrowerId = 1, PickupBranchId = 1, Status = HoldStatus.Waiting, CreatedAtUtc = created },
            new Hold { Id = 4, ItemId = 9, BorrowerId = 1, PickupBranchId = 1, Status = HoldStatus.Cancelled, CreatedAtUtc = created },
            new Hold { Id = 5, ItemId = 10, BorrowerId = 2, PickupBranchId = 1, Status = HoldStatus.Waiting, CreatedAtUtc = created },
            new Hold { Id = 6, ItemId = 11, BorrowerId = 1, PickupBranchId = 2, Status = HoldStatus.Allocated, CreatedAtUtc = created });
        db.ReceptionDesks.Add(new ReceptionDesk { Id = 1, BranchId = 1, DeskCode = "DAR-D01", Name = "Demo desk", IsActive = true });
        db.ItemTransfers.AddRange(new ItemTransfer { Id = 1, ItemId = 4, OriginBranchId = 1, DestinationBranchId = 2, DispatchDeskId = 1, Status = TransferStatus.InTransit, DispatchedByUserId = "test", RowVersion = Array.Empty<byte>() },
            new ItemTransfer { Id = 2, ItemId = 12, OriginBranchId = 1, DestinationBranchId = 2, DispatchDeskId = 1, Status = TransferStatus.InTransit, DispatchedByUserId = "test", RowVersion = Array.Empty<byte>() });
        var borrowed = DateTime.Parse("2026-10-01T00:00:00Z").ToUniversalTime();
        db.Loans.AddRange(new Loan { Id = 1, ItemId = 6, BorrowerId = 1, BorrowedDate = borrowed, DueDate = new(2026, 10, 15) },
            new Loan { Id = 2, ItemId = 8, BorrowerId = 1, BorrowedDate = borrowed, DueDate = new(2026, 10, 17) },
            new Loan { Id = 3, ItemId = 9, BorrowerId = 1, BorrowedDate = borrowed, DueDate = new(2026, 10, 11), ReturnedDate = created.AddDays(1), FineAmount = 6m, FinePaid = false },
            new Loan { Id = 4, ItemId = 10, BorrowerId = 3, BorrowedDate = borrowed, DueDate = new(2026, 10, 13), ReturnedDate = created.AddDays(1), FineAmount = 3m, FinePaid = true,
                FinePaidDate = DateTime.Parse("2026-10-15T15:00:00Z").ToUniversalTime() });
        await db.SaveChangesAsync(); db.ChangeTracker.Clear();
    }
}
