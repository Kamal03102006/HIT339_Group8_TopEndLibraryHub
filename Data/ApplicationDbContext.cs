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

        public DbSet<Item> Items => Set<Item>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Music> MusicItems => Set<Music>();
        public DbSet<Toy> Toys => Set<Toy>();

        public DbSet<Borrower> Borrowers => Set<Borrower>();
        public DbSet<Loan> Loans => Set<Loan>();

        public DbSet<Hold> Holds => Set<Hold>();
        public DbSet<NotificationLog> NotificationLogs
            => Set<NotificationLog>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Preserve the existing Holds and Notifications setup.
            builder.ApplyConfiguration(new HoldConfiguration());
            builder.ApplyConfiguration(new NotificationLogConfiguration());

            // Item inheritance.
            builder.Entity<Item>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<Book>("Book")
                .HasValue<Music>("Music")
                .HasValue<Toy>("Toy");

            // Unique library item code.
            builder.Entity<Item>()
                .HasIndex(item => item.LibraryCode)
                .IsUnique();

            // Unique borrower membership number.
            builder.Entity<Borrower>()
                .HasIndex(borrower => borrower.MembershipNumber)
                .IsUnique();

            // Unique borrower email.
            builder.Entity<Borrower>()
                .HasIndex(borrower => borrower.Email)
                .IsUnique();

            // Connect a borrower to an authenticated Identity user.
            builder.Entity<Borrower>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Borrower>(
                    borrower => borrower.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One Identity account cannot be linked
            // to multiple borrowers.
            builder.Entity<Borrower>()
                .HasIndex(borrower => borrower.IdentityUserId)
                .IsUnique()
                .HasFilter("[IdentityUserId] IS NOT NULL");

            // Loan relationships.
            builder.Entity<Loan>()
                .HasOne(loan => loan.Item)
                .WithMany()
                .HasForeignKey(loan => loan.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Loan>()
                .HasOne(loan => loan.Borrower)
                .WithMany(borrower => borrower.Loans)
                .HasForeignKey(loan => loan.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Fine amount precision.
            builder.Entity<Loan>()
                .Property(loan => loan.FineAmount)
                .HasPrecision(10, 2);

            // Prevent two active loans for the same item.
            builder.Entity<Loan>()
                .HasIndex(loan => loan.ItemId)
                .IsUnique()
                .HasFilter("[ReturnedDate] IS NULL");
        }
    }
}