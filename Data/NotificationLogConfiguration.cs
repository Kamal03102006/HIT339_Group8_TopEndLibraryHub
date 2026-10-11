using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Data
{
    public class NotificationLogConfiguration
        : IEntityTypeConfiguration<NotificationLog>
    {
        public void Configure(EntityTypeBuilder<NotificationLog> builder)
        {
            builder.ToTable("NotificationLogs");

            builder.HasOne(log => log.Borrower)
                .WithMany()
                .HasForeignKey(log => log.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(log => log.Loan)
                .WithMany()
                .HasForeignKey(log => log.LoanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(log => log.Hold)
                .WithMany()
                .HasForeignKey(log => log.HoldId)
                .OnDelete(DeleteBehavior.Restrict);

            // One simulation per event and channel, including across restarts.
            builder.HasIndex(log => new { log.EventKey, log.Channel })
                .HasDatabaseName("IX_NotificationLogs_EventChannel")
                .IsUnique();

            builder.HasIndex(log => new { log.BorrowerId, log.CreatedAtUtc })
                .HasDatabaseName("IX_NotificationLogs_BorrowerHistory");
        }
    }
}
