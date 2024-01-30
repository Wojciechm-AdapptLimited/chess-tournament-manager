using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ChessTournamentManager.Infra.Seeds;

public static class DefaultClaims
{
    public static async Task SeedClaimsAsync(this RoleManager<IdentityRole<Guid>> roleManager)
    {
        var adminRole = await roleManager.FindByNameAsync(Role.Admin.ToString());
        var playerRole = await roleManager.FindByNameAsync(Role.Player.ToString());
        var organizerRole = await roleManager.FindByNameAsync(Role.Organizer.ToString());
        var refereeRole = await roleManager.FindByNameAsync(Role.Referee.ToString());
        
        if (adminRole is not null)
        {
            await roleManager.SeedForAdminAsync(adminRole);
        }
        
        if (playerRole is not null)
        {
            await roleManager.SeedForPlayerAsync(playerRole);
        }
        
        if (organizerRole is not null)
        {
            await roleManager.SeedForOrganizerAsync(organizerRole);
        }
        
        if (refereeRole is not null)
        {
            await roleManager.SeedForRefereeAsync(refereeRole);
        }
    }
    
    private static async Task SeedForAdminAsync(this RoleManager<IdentityRole<Guid>> roleManager, IdentityRole<Guid> adminRole)
    {
        var allClaims = await roleManager.GetClaimsAsync(adminRole);

        foreach (var module in Enum.GetValues<Module>())
        {
            var permissions = Permissions.GeneratePermissionsForModule(module);
            
            foreach (var permission in permissions)
            {
                if (allClaims.Any(c => c.Type == permission))
                {
                    continue;
                }

                await roleManager.AddClaimAsync(adminRole, new Claim(permission, "true"));
            }
        }
    }

    private static async Task SeedForPlayerAsync(this RoleManager<IdentityRole<Guid>> roleManager, IdentityRole<Guid> playerRole)
    {
        var permissions = new[]
        {
            Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Create),
            Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Read),
            Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Update),
            Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Delete),
            Permissions.GeneratePermission(Module.Games, Permission.Read),
            Permissions.GeneratePermission(Module.Tournaments, Permission.Read),
            Permissions.GeneratePermission(Module.Results, Permission.Read)
        };
        var allClaims = await roleManager.GetClaimsAsync(playerRole);
        
        foreach (var permission in permissions)
        {
            if (allClaims.Any(c => c.Type == permission))
            {
                continue;
            }

            await roleManager.AddClaimAsync(playerRole, new Claim(permission, "true"));
        }
    }
    
    private static async Task SeedForOrganizerAsync(this RoleManager<IdentityRole<Guid>> roleManager, IdentityRole<Guid> organizerRole)
    {
        var permissions = new[]
        {
            Permissions.GeneratePermission(Module.Tournaments, Permission.Create),
            Permissions.GeneratePermission(Module.Tournaments, Permission.Read),
            Permissions.GeneratePermission(Module.Tournaments, Permission.Update),
            Permissions.GeneratePermission(Module.Tournaments, Permission.Delete),
            Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Create),
            Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Read),
            Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Update),
            Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Delete),
            Permissions.GeneratePermission(Module.Games, Permission.Create),
            Permissions.GeneratePermission(Module.Games, Permission.Read),
            Permissions.GeneratePermission(Module.Games, Permission.Update),
            Permissions.GeneratePermission(Module.Games, Permission.Delete),
        };
        var allClaims = await roleManager.GetClaimsAsync(organizerRole);
        
        foreach (var permission in permissions)
        {
            if (allClaims.Any(c => c.Type == permission))
            {
                continue;
            }

            await roleManager.AddClaimAsync(organizerRole, new Claim(permission, "true"));
        }
    }
    
    private static async Task SeedForRefereeAsync(this RoleManager<IdentityRole<Guid>> roleManager, IdentityRole<Guid> refereeRole)
    {
        var permissions = new[]
        {
            Permissions.GeneratePermission(Module.Tournaments, Permission.Read),
            Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Read),
            Permissions.GeneratePermission(Module.Games, Permission.Read),
            Permissions.GeneratePermission(Module.Games, Permission.Update),
            Permissions.GeneratePermission(Module.Results, Permission.Create),
            Permissions.GeneratePermission(Module.Results, Permission.Read),
            Permissions.GeneratePermission(Module.Results, Permission.Update),
            Permissions.GeneratePermission(Module.Results, Permission.Delete),
        };
        var allClaims = await roleManager.GetClaimsAsync(refereeRole);
        
        foreach (var permission in permissions)
        {
            if (allClaims.Any(c => c.Type == permission))
            {
                continue;
            }

            await roleManager.AddClaimAsync(refereeRole, new Claim(permission, "true"));
        }
    }
}