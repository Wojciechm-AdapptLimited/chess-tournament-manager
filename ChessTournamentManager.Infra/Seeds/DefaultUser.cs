using ChessTournamentManager.Core.User;
using Microsoft.AspNetCore.Identity;

namespace ChessTournamentManager.Infra.Seeds;

public static class DefaultUser
{
    public static async Task SeedAsync(this UserManager<ApplicationUser> userManager)
    {
        var defaultUser = new ApplicationUser
        {   
            FirstName = string.Empty,
            LastName = string.Empty,
            UserName = "admin@localhost",
            Email = "admin@localhost",
            EmailConfirmed = true
        };

        if (userManager.Users.Any(u => u.Id == defaultUser.Id))
        {
            return;
        }

        var user = await userManager.FindByEmailAsync(defaultUser.Email);

        if (user is null)
        {
            await userManager.CreateAsync(defaultUser, "Admin123!");
            await userManager.AddToRoleAsync(defaultUser, Role.Admin.ToString());
        }
    }
}