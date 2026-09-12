using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Data
{
    public static class DbInitializer
    {
        public static async Task InitialiseAsync(
            IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            // Automatically apply pending migrations.
            await context.Database.MigrateAsync();

            string[] roles =
            {
                "Admin",
                "Reception",
                "Manager"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(
                            "; ",
                            result.Errors.Select(error =>
                                error.Description));

                        throw new InvalidOperationException(
                            $"Could not create role {role}: {errors}");
                    }
                }
            }

            await CreateStaffAccountAsync(
                userManager,
                "admin@topendlibrary.local",
                "Admin#2026!",
                "Admin");

            await CreateStaffAccountAsync(
                userManager,
                "reception@topendlibrary.local",
                "Reception#2026!",
                "Reception");

            await CreateStaffAccountAsync(
                userManager,
                "manager@topendlibrary.local",
                "Manager#2026!",
                "Manager");

            await SeedDemonstrationDataAsync(context);
        }

        private static async Task CreateStaffAccountAsync(
            UserManager<IdentityUser> userManager,
            string email,
            string password,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(
                    user,
                    password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        createResult.Errors.Select(error =>
                            error.Description));

                    throw new InvalidOperationException(
                        $"Could not create {email}: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    user,
                    role);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleResult.Errors.Select(error =>
                            error.Description));

                    throw new InvalidOperationException(
                        $"Could not assign {email} to {role}: {errors}");
                }
            }
        }

        private static async Task SeedDemonstrationDataAsync(
            ApplicationDbContext context)
        {
            var today = DateTime.UtcNow.Date;

            var borrower = await context.Borrowers
                .FirstOrDefaultAsync(b =>
                    b.MembershipNumber == "MBR-0001");

            if (borrower is null)
            {
                borrower = new Borrower
                {
                    MembershipNumber = "MBR-0001",
                    FullName = "Leah Johnson",
                    Email = "leah.johnson@example.com",
                    Phone = "0412345678",
                    Address = "12 Smith Street, Darwin NT 0800",
                    DateRegistered = today.AddDays(-90),
                    IsActive = true
                };

                context.Borrowers.Add(borrower);
            }

            var book = await context.Books
                .FirstOrDefaultAsync(item =>
                    item.LibraryCode == "BK-0001");

            if (book is null)
            {
                book = new Book
                {
                    LibraryCode = "BK-0001",
                    Name = "Discovering the Top End",
                    Description =
                        "A practical guide to the landscapes, wildlife and communities of Northern Australia.",
                    Status = ItemStatus.Available,
                    DateAdded = today.AddDays(-60),
                    Author = "Maya Thompson",
                    BookGenre = "Australian Travel",
                    PublicationYear = 2024,
                    Publisher = "Northern Territory Press"
                };

                context.Books.Add(book);
            }

            var music = await context.MusicItems
                .FirstOrDefaultAsync(item =>
                    item.LibraryCode == "MU-0001");

            if (music is null)
            {
                music = new Music
                {
                    LibraryCode = "MU-0001",
                    Name = "Sounds of the Wet Season",
                    Description =
                        "An atmospheric music collection inspired by the Top End wet season.",
                    Status = ItemStatus.Available,
                    DateAdded = today.AddDays(-45),
                    Artist = "Top End Sound Collective",
                    MusicGenre = "Ambient",
                    ReleaseYear = 2025,
                    Format = "Compact Disc (CD)"
                };

                context.MusicItems.Add(music);
            }

            var toy = await context.Toys
                .FirstOrDefaultAsync(item =>
                    item.LibraryCode == "TY-0001");

            if (toy is null)
            {
                toy = new Toy
                {
                    LibraryCode = "TY-0001",
                    Name = "Build the Top End",
                    Description =
                        "A durable construction set designed for creative and cooperative play.",
                    Status = ItemStatus.Available,
                    DateAdded = today.AddDays(-30),
                    ToyType = "Construction Set",
                    RecommendedAge = "6-10 years",
                    Manufacturer = "Northern Play Co.",
                    Material = "Recycled ABS Plastic"
                };

                context.Toys.Add(toy);
            }

            await context.SaveChangesAsync();

            var bookHasHistory = await context.Loans
                .AnyAsync(loan => loan.ItemId == book.Id);

            if (!bookHasHistory)
            {
                context.Loans.Add(new Loan
                {
                    ItemId = book.Id,
                    BorrowerId = borrower.Id,
                    BorrowedDate = today.AddDays(-30),
                    DueDate = today.AddDays(-16),
                    ReturnedDate = today.AddDays(-18),
                    FineAmount = 0,
                    FinePaid = true
                });
            }

            var musicHasHistory = await context.Loans
                .AnyAsync(loan => loan.ItemId == music.Id);

            if (!musicHasHistory)
            {
                context.Loans.Add(new Loan
                {
                    ItemId = music.Id,
                    BorrowerId = borrower.Id,
                    BorrowedDate = today.AddDays(-3),
                    DueDate = today.AddDays(11),
                    FineAmount = 0,
                    FinePaid = false
                });

                music.Status = ItemStatus.Borrowed;
            }

            var toyHasHistory = await context.Loans
                .AnyAsync(loan => loan.ItemId == toy.Id);

            if (!toyHasHistory)
            {
                context.Loans.Add(new Loan
                {
                    ItemId = toy.Id,
                    BorrowerId = borrower.Id,
                    BorrowedDate = today.AddDays(-21),
                    DueDate = today.AddDays(-7),
                    FineAmount = 0,
                    FinePaid = false
                });

                toy.Status = ItemStatus.Borrowed;
            }

            await context.SaveChangesAsync();
        }
    }
}