namespace ChessTournamentManager.Data;

public static class Permissions
{
    private const string GroupName = "Permissions";
    
    public enum Module
    {
        Users,
    }
    
    public enum Permission
    {
        Create,
        Read,
        Update,
        Delete
    }
    
    public static string[] GeneratePermissionsForModule(Module module)
    {
        return new[]
        {
            GeneratePermission(module, Permission.Create),
            GeneratePermission(module, Permission.Read),
            GeneratePermission(module, Permission.Update),
            GeneratePermission(module, Permission.Delete)
        }; 
    }
    public static string GeneratePermission(Module module, Permission permission) => $"{GroupName}.{module}.{permission}";
}