using Azure.Communication.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using ChessTournamentManager.Core.User;
using Microsoft.Extensions.Configuration;

namespace ChessTournamentManager.Infra;

public sealed class IdentityEmailSender(IWebHostEnvironment environment, EmailClient emailClient, IConfiguration configuration) : IEmailSender<ApplicationUser>
{
    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var body = GetTemplate();
        body = body.Replace("{{header}}", "Confirm your email");
        body = body.Replace("{{body}}",
            "You're almost ready to start your journey with us. Just click the button below to confirm your email address.");
        body = body.Replace("{{button}}", $"<a href='{confirmationLink}' class='button'>Confirm Email</a>");
        
        return emailClient.SendAsync(Azure.WaitUntil.Completed, configuration["Azure:Communication:Email:DefaultSender"], email, "Confirm your email", body);
    }

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var body = GetTemplate();
        body = body.Replace("{{header}}", "Reset your password");
        body = body.Replace("{{body}}",
            "We received a request to reset your password for your Chess World account. Simply click the button below to set a new password. This link will expire in 24 hours.");
        body = body.Replace("{{button}}", $"<a href='{resetLink}' class='button'>Reset Password</a>");
        
        return emailClient.SendAsync(Azure.WaitUntil.Completed, configuration["Azure:Communication:Email:DefaultSender"], email, "Reset your password", body); 
    }

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var body = GetTemplate();
        body = body.Replace("{{header}}", "Reset your password");
        body = body.Replace("{{body}}",
            "We received a request to reset your password for your Chess World account. Please use the following code to reset your password.");
        body = body.Replace("{{button}}", $"<p class='code'>{resetCode}</p>");
        
        return emailClient.SendAsync(Azure.WaitUntil.Completed, configuration["Azure:Communication:Email:DefaultSender"], email, "Reset your password", body);
    }

    private string GetTemplate()
    {
        var path = Path.Combine(environment.ContentRootPath, "Templates", "EmailTemplate.html");
        return File.ReadAllText(path);
    }
}
