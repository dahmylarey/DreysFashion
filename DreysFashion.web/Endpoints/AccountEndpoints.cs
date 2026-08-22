using DreysFashion.web.Models;
using Microsoft.AspNetCore.Identity;

namespace DreysFashion.web.Endpoints
{
    /// <summary>
    /// Provides HTTP endpoints for customer account operations.
    /// </summary>
    public static class AccountEndpoints
    {
        /// <summary>
        /// Maps the account-related HTTP endpoints.
        /// </summary>
        /// <param name="app">
        /// The application's endpoint route builder.
        /// </param>
        public static void MapAccountEndpoints(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("/account/login", LoginAsync);
            app.MapPost("/account/logout", LogoutAsync);
        }

        /// <summary>
        /// Authenticates a customer and creates the Identity cookie.
        /// </summary>
        private static async Task<IResult> LoginAsync(
            LoginRequest request,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return Results.BadRequest(
                    new { message = "Email and password are required." });
            }

            var user = await userManager.FindByEmailAsync(
                request.Email);

            if (user == null)
            {
                return Results.Unauthorized();
            }

            var result = await signInManager.PasswordSignInAsync(
                user,
                request.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Results.Unauthorized();
            }

            return Results.Ok();
        }

        /// <summary>
        /// Signs the current customer out.
        /// </summary>
        private static async Task<IResult> LogoutAsync(
            SignInManager<ApplicationUser> signInManager)
        {
            await signInManager.SignOutAsync();

            return Results.Ok();
        }

        /// <summary>
        /// Represents the login request sent by the client.
        /// </summary>
        private sealed record LoginRequest(
            string Email,
            string Password);
    }
}