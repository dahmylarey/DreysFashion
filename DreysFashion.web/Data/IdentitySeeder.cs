using DreysFashion.web.Models;
using Microsoft.AspNetCore.Identity;

namespace DreysFashion.web.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager =
                services.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                services.GetRequiredService<UserManager<ApplicationUser>>();

            const string adminRole = "Admin";

            const string adminEmail =
                "oladeleoluwadamilare@gmail.com";

            // Create Admin role if it does not exist.
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(adminRole));
            }

            // Find the existing account.
            var adminUser =
                await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                throw new InvalidOperationException(
                    $"Admin account '{adminEmail}' was not found.");
            }

            // Add the account to the Admin role.
            if (!await userManager.IsInRoleAsync(
                    adminUser,
                    adminRole))
            {
                var result =
                    await userManager.AddToRoleAsync(
                        adminUser,
                        adminRole);

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Unable to assign Admin role: {errors}");
                }
            }
        }
    }
}