
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Library catalogue
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Music> MusicItems => Set<Music>();
        public DbSet<Toy> Toys => Set<Toy>();

        // Borrowing
        public DbSet<Borrower> Borrowers => Set<Borrower>();
        public DbSet<Loan> Loans => Set<Loan>();

        // Holds and notifications
        public DbSet<Hold> Holds => Set<Hold>();
        public DbSet<NotificationLog> NotificationLogs
            => Set<NotificationLog>();

        // Branch and transfer integration
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<ReceptionDesk> ReceptionDesks
            => Set<ReceptionDesk>();
        public DbSet<ItemTransfer> ItemTransfers
            => Set<ItemTransfer>();

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Each physical branch has a unique code.
            builder.Entity<Branch>()
                .HasIndex(branch => branch.BranchCode)
                .IsUnique();

            // Preserve teammate's Holds and Notifications
            builder.ApplyConfiguration(
                new HoldConfiguration());

            builder.ApplyConfiguration(
                new NotificationLogConfiguration());

            // Item inheritance
            builder.Entity<Item>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<Book>("Book")
                .HasValue<Music>("Music")
                .HasValue<Toy>("Toy");

            builder.Entity<Item>()
                .HasIndex(i => i.LibraryCode)
                .IsUnique();

            // Borrower membership
            builder.Entity<Borrower>()
                .HasIndex(b => b.MembershipNumber)
                .IsUnique();

            builder.Entity<Borrower>()
                .HasIndex(b => b.Email)
                .IsUnique();

            // Kiosk Identity-to-Borrower relationship
            builder.Entity<Borrower>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Borrower>(
                    b => b.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Borrower>()
                .HasIndex(b => b.IdentityUserId)
                .IsUnique()
                .HasFilter("[IdentityUserId] IS NOT NULL");

            // Loan relationships
            builder.Entity<Loan>()
                .HasOne(l => l.Item)
                .WithMany()
                .HasForeignKey(l => l.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Loan>()
                .HasOne(l => l.Borrower)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Loan>()
                .Property(l => l.FineAmount)
                .HasPrecision(10, 2);

            // Prevent duplicate active loans
            builder.Entity<Loan>()
                .HasIndex(l => l.ItemId)
                .IsUnique()
                .HasFilter("[ReturnedDate] IS NULL");

            // Branch configuration
            builder.Entity<Branch>()
                .HasIndex(b => b.BranchCode)
                .IsUnique();

            builder.Entity<ReceptionDesk>()
                .HasIndex(d => d.DeskCode)
                .IsUnique();

            builder.Entity<Item>()
                .HasOne(i => i.Branch)
                .WithMany()
                .HasForeignKey(i => i.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReceptionDesk>()
                .HasOne(d => d.Branch)
                .WithMany()
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transfer relationships
            builder.Entity<ItemTransfer>()
                .HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey(t => t.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemTransfer>()
                .HasOne(t => t.OriginBranch)
                .WithMany()
                .HasForeignKey(t => t.OriginBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemTransfer>()
                .HasOne(t => t.DestinationBranch)
                .WithMany()
                .HasForeignKey(t => t.DestinationBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemTransfer>()
                .HasOne(t => t.DispatchDesk)
                .WithMany()
                .HasForeignKey(t => t.DispatchDeskId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemTransfer>()
                .HasOne(t => t.ReceivingDesk)
                .WithMany()
                .HasForeignKey(t => t.ReceivingDeskId)
                .OnDelete(DeleteBehavior.Restrict);

            // One in-transit transfer per item
            builder.Entity<ItemTransfer>()
                .HasIndex(t => t.ItemId)
                .IsUnique()
                .HasFilter("[Status] = 1");
        }
    }
}