using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;
using ChessTournamentManager.Core.Base;
using Microsoft.AspNetCore.Identity;

namespace ChessTournamentManager.Core.User;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser(string firstName, string lastName) : IdentityUser<Guid>, IAuditable, ISoftDeletable
{
    private string _firstName =
        Guard.Against.LengthOutOfRange(Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName)), 1, 50,
            nameof(firstName));
    private string _lastName =
        Guard.Against.LengthOutOfRange(Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName)), 1, 50,
            nameof(lastName));

    [MaxLength(50)]
    public string FirstName
    {
        get => _firstName;
        set => _firstName = Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }
    
    [MaxLength(50)]
    public string LastName 
    {
        get => _lastName;
        set => _lastName = Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }
    
    public bool IsDeleted { get; set; }
    
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
