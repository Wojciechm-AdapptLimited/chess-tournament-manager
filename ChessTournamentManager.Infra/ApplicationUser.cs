using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ChessTournamentManager.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [MaxLength(50)]
    public string FirstName { get; set; } = default!;
    
    [MaxLength(50)]
    public string LastName { get; set; } = default!;
    
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}

