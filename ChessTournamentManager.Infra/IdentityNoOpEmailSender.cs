using ChessTournamentManager.Core.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ChessTournamentManager.Data;

namespace ChessTournamentManager.Components.Account;

// Remove the "else if (EmailSender is IdentityNoOpEmailSender)" block from RegisterConfirmation.razor after updating with a real implementation.
internal sealed class IdentityNoOpEmailSender(IWebHostEnvironment environment) : IEmailSender<ApplicationUser>
{
    private readonly IEmailSender _emailSender = new NoOpEmailSender();

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var body = GetTemplate();
        body = body.Replace("{{header}}", "Confirm your email");
        body = body.Replace("{{body}}",
            "You're almost ready to start your journey with us. Just click the button below to confirm your email address.");
        body = body.Replace("{{button}}", $"<a href='{confirmationLink}' class='button'>Confirm Email</a>");
        
        return _emailSender.SendEmailAsync(email, "Confirm your email", body);
    }

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var body = GetTemplate();
        body = body.Replace("{{header}}", "Reset your password");
        body = body.Replace("{{body}}",
            "We received a request to reset your password for your Chess World account. Simply click the button below to set a new password. This link will expire in 24 hours.");
        body = body.Replace("{{button}}", $"<a href='{resetLink}' class='button'>Reset Password</a>");
        
        return _emailSender.SendEmailAsync(email, "Reset your password", body);
    }

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var body = GetTemplate();
        body = body.Replace("{{header}}", "Reset your password");
        body = body.Replace("{{body}}",
            "We received a request to reset your password for your Chess World account. Please use the following code to reset your password.");
        body = body.Replace("{{button}}", $"<p class='code'>{resetCode}</p>");
        
        return _emailSender.SendEmailAsync(email, "Reset your password", body);
    }

    private string GetTemplate()
    {
        var path = Path.Combine(environment.ContentRootPath, "Templates", "EmailTemplate.html");
        return File.ReadAllText(path);
    }
}
