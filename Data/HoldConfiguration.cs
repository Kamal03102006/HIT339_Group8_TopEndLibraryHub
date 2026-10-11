using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Data
{
    public class HoldConfiguration : IEntityTypeConfiguration<Hold>
    {
        public void Configure(EntityTypeBuilder<Hold> builder)
        {
            builder.ToTable("Holds");

            builder.Property(hold => hold.Status)
                .HasConversion<int>();

            builder.Property(hold => hold.RowVersion)
                .IsRowVersion();

            builder.HasOne(hold => hold.Item)
                .WithMany()
                .HasForeignKey(hold => hold.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(hold => hold.Borrower)
                .WithMany()
                .HasForeignKey(hold => hold.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Waiting, Allocated and ReadyForPickup are active.
            builder.HasIndex(hold => new
            { hold.ItemId, hold.BorrowerId })
                .HasDatabaseName("IX_Holds_OneActiveRequestPerBorrower")
                .IsUnique()
                .HasFilter("[Status] IN (0, 1, 2)");

            // One pickup allocation per physical item.
            builder.HasIndex(hold => hold.ItemId)
                .HasDatabaseName("IX_Holds_OneAllocationPerItem")
                .IsUnique()
                .HasFilter("[Status] IN (1, 2)");

            builder.HasIndex(hold => new
            { hold.ItemId, hold.Status, hold.CreatedAtUtc, hold.Id })
                .HasDatabaseName("IX_Holds_QueueOrder");
        }
    }
}