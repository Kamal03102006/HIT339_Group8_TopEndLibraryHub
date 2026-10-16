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
            await SeedBranchesAndDesksAsync(context);
            await SeedDemonstrationDataAsync(context);
            if (scope.ServiceProvider.GetRequiredService<IHostEnvironment>().IsDevelopment())
                await SeedDemoPatronAsync(context, userManager);
        }

        // A separate fictional membership demonstrates verified ownership.
        // Never claim an existing member record using a matching email or card number.
        private static async Task SeedDemoPatronAsync(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            const string email = "patron@topendlibrary.local";
            const string membership = "MBR-8001";
            var borrower = await context.Borrowers.SingleOrDefaultAsync(
                member => member.MembershipNumber == membership);
            if (borrower is not null &&
                (borrower.Email != email || borrower.FullName != "Sky Nguyen"))
                return; // Preserve any member already using this demonstration code.
            if (borrower is null && await context.Borrowers.AnyAsync(member => member.Email == email))
                return;

            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, "Patron#2026!");
                if (!result.Succeeded)
                    throw new InvalidOperationException("The development patron account could not be created: " +
                        string.Join("; ", result.Errors.Select(error => error.Description)));
            }
            if (borrower?.IdentityUserId is not null && borrower.IdentityUserId != user.Id) return;
            if (await context.Borrowers.AnyAsync(member => member.IdentityUserId == user.Id &&
                member.MembershipNumber != membership)) return;
            if (borrower is null)
            {
                borrower = new Borrower
                {
                    MembershipNumber = membership, FullName = "Sky Nguyen", Email = email,
                    Phone = "0400000801", Address = "Fictional demonstration member, Darwin NT",
                    DateRegistered = DateTime.UtcNow, IsActive = true
                };
                context.Borrowers.Add(borrower);
            }
            borrower.IdentityUserId = user.Id;
            await context.SaveChangesAsync();
        }
          private static async Task SeedBranchesAndDesksAsync(
    ApplicationDbContext context)
        {
            var locations = new[]
            {
        new
        {
            BranchCode = "DAR",
            Name = "Darwin Hub",
            Address = "Fictional demonstration location in Darwin NT",
            DeskCode = "DAR-D01"
        },
        new
        {
            BranchCode = "PAL",
            Name = "Palmerston Hub",
            Address = "Fictional demonstration location in Palmerston NT",
            DeskCode = "PAL-D01"
        },
        new
        {
            BranchCode = "CAS",
            Name = "Casuarina Hub",
            Address = "Fictional demonstration location in Casuarina NT",
            DeskCode = "CAS-D01"
        }
    };

            foreach (var location in locations)
            {
                var branch = await context.Branches
                    .SingleOrDefaultAsync(b =>
                        b.BranchCode == location.BranchCode);

                if (branch is null)
                {
                    branch = new Branch
                    {
                        BranchCode = location.BranchCode,
                        Name = location.Name,
                        Address = location.Address,
                        IsActive = true
                    };

                    context.Branches.Add(branch);
                    await context.SaveChangesAsync();
                }

                var desk = await context.ReceptionDesks
                    .SingleOrDefaultAsync(d =>
                        d.DeskCode == location.DeskCode);

                if (desk is null)
                {
                    context.ReceptionDesks.Add(new ReceptionDesk
                    {
                        DeskCode = location.DeskCode,
                        Name = $"{location.Name} Reception",
                        BranchId = branch.Id,
                        IsActive = true
                    });
                }
                else if (desk.BranchId != branch.Id)
                {
                    throw new InvalidOperationException(
                        $"Desk {location.DeskCode} belongs to the wrong branch. " +
                        "Check its branch assignment.");
                }
            }

            await context.SaveChangesAsync();
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
            var darwinBranchId = await context.Branches
    .Where(b => b.BranchCode == "DAR")
    .Select(b => b.Id)
    .SingleAsync();
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
                    BranchId = darwinBranchId,
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
                    BranchId = darwinBranchId,
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
                    BranchId = darwinBranchId,
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
