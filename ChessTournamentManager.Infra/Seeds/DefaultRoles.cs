using Microsoft.AspNetCore.Identity;

namespace ChessTournamentManager.Infra.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(this RoleManager<IdentityRole<Guid>> roleManager)
    {
        foreach (var role in Enum.GetNames<Role>())
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }
}