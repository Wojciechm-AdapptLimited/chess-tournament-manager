using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChessTournamentManager.Core.Base;
using Microsoft.AspNetCore.Identity;

namespace ChessTournamentManager.Core.User;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<Guid>, IAuditable, ISoftDeletable
{
    [MaxLength(50)]
    public string FirstName { get; set; } = default!;
    
    [MaxLength(50)]
    public string LastName { get; set; } = default!;
    
    public bool IsDeleted { get; set; }
    
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
