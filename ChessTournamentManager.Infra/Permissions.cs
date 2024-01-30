using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ChessTournamentManager.Infra;

public static class Permissions
{
    private const string GroupName = "Permissions";

    public static string[] GeneratePermissionsForModule(Module module)
    {
        return
        [
            GeneratePermission(module, Permission.Create),
            GeneratePermission(module, Permission.Read),
            GeneratePermission(module, Permission.Update),
            GeneratePermission(module, Permission.Delete)
        ]; 
    }
    public static string GeneratePermission(Module module, Permission permission) => $"{GroupName}.{module}.{permission}";
    
    public static void AddPermissions(this AuthorizationPolicyBuilder builder, Module module, Permission permission)
    {
        builder.AddRequirements(new ClaimsAuthorizationRequirement(GeneratePermission(module, permission), ["true"]));
    }
}

public enum Permission
{
    Create,
    Read,
    Update,
    Delete
}