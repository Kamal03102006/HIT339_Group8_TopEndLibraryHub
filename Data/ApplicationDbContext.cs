using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Data
{
    public class ApplicationDbContext
        : IdentityDbContext
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new HoldConfiguration());

            // Store all item types in one table while preserving inheritance.
            builder.Entity<Item>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<Book>("Book")
                .HasValue<Music>("Music")
                .HasValue<Toy>("Toy");

            // Every physical library item must have a unique code.
            builder.Entity<Item>()
                .HasIndex(item => item.LibraryCode)
                .IsUnique();

            builder.Entity<Borrower>()
                .HasIndex(borrower => borrower.MembershipNumber)
                .IsUnique();

            builder.Entity<Borrower>()
                .HasIndex(borrower => borrower.Email)
                .IsUnique();

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

            builder.Entity<Loan>()
                .Property(loan => loan.FineAmount)
                .HasPrecision(10, 2);
        }
    }
}