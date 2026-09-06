using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TopEndLibraryHub.Data
{
    public static class DbInitializer
    {
        public static async Task InitialiseAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var context = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

            // Automatically apply pending migrations for easy installation.
            await context.Database.MigrateAsync();

            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Admin", "Reception", "Manager" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult =
                        await roleManager.CreateAsync(new IdentityRole(role));

                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(
                            "; ",
                            roleResult.Errors.Select(error => error.Description));

                        throw new InvalidOperationException(
                            $"Could not create the {role} role: {errors}");
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

                var createResult =
                    await userManager.CreateAsync(user, password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        createResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Could not create {email}: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                var roleResult =
                    await userManager.AddToRoleAsync(user, role);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Could not assign {email} to {role}: {errors}");
                }
            }
        }
    }
}