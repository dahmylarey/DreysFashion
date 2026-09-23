using DreysFashion.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;

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

            var configuration =
                services.GetRequiredService<IConfiguration>();

            const string adminRole = "Admin";

            // Prefer seed values from configuration but fall back to the
            // original hard-coded email. Provide a fallback password as
            // well — it's strongly recommended to set this via config or
            // environment in production.
            var adminEmail = configuration["SeedAdmin:Email"] ??
                "oladeleoluwadamilare@gmail.com";

            var adminPassword = configuration["SeedAdmin:Password"] ??
                "ChangeMe@123!";

            // Ensure Admin role exists.
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                var roleResult = await roleManager.CreateAsync(
                    new IdentityRole(adminRole));

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Unable to create role '{adminRole}': {errors}");
                }
            }

            // Try to find a user that already has the seeded email.
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // If no user with the seeded email exists, try to find any user
            // that is already in the Admin role and update its email.
            if (adminUser == null)
            {
                var adminsInRole = await userManager.GetUsersInRoleAsync(adminRole);
                var existingAdmin = adminsInRole.FirstOrDefault();

                if (existingAdmin != null)
                {
                    existingAdmin.Email = adminEmail;
                    existingAdmin.UserName = adminEmail;
                    existingAdmin.EmailConfirmed = true;

                    var updateResult = await userManager.UpdateAsync(existingAdmin);
                    if (!updateResult.Succeeded)
                    {
                        var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Unable to update existing admin email: {errors}");
                    }

                    adminUser = existingAdmin;
                }
            }

            // If still not found, create a new admin user with the seeded email
            // and a password (from config or the fallback). The caller should
            // override the password via configuration for production.
            if (adminUser == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(newUser, adminPassword);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Unable to create admin user: {errors}");
                }

                adminUser = newUser;
            }

            // Ensure the admin user has the Admin role.
            if (!await userManager.IsInRoleAsync(adminUser, adminRole))
            {
                var addRoleResult = await userManager.AddToRoleAsync(adminUser, adminRole);
                if (!addRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Unable to assign Admin role: {errors}");
                }
            }

            // Optionally ensure the admin user has the configured password.
            // If the account was created above we already set the password.
            // If we updated an existing account, reset its password to the
            // configured value so the seed is deterministic.
            var hasPassword = await userManager.HasPasswordAsync(adminUser);
            if (hasPassword)
            {
                // Reset password to the configured one to ensure seed state.
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                var resetResult = await userManager.ResetPasswordAsync(adminUser, token, adminPassword);
                if (!resetResult.Succeeded)
                {
                    var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Unable to reset admin password: {errors}");
                }
            }
        }
    }
}