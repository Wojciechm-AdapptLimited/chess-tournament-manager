using System.Security.Claims;
using System.Text.Json;
using ChessTournamentManager.Account.Pages;
using ChessTournamentManager.Account.Pages.Manage;
using ChessTournamentManager.Core.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Serilog;

namespace ChessTournamentManager.Endpoints;

internal static class IdentityEndpoints
{
    // These endpoints are required by the Identity Razor components defined in the /Components/Account/Pages directory of this project.
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        
        endpoints.MapPost("/logout", OnLogout);

        var accountGroup = endpoints.MapGroup("/account").RequireAuthorization();

        accountGroup.MapPost("/download-data", OnDownloadingPersonalData);

        return accountGroup;
    }

    private static async Task<IResult> OnDownloadingPersonalData(HttpContext context, [FromServices] UserManager<ApplicationUser> userManager, [FromServices] AuthenticationStateProvider authenticationStateProvider)
    {
        var user = await userManager.GetUserAsync(context.User);
        if (user is null)
        {
            return Results.NotFound($"Unable to load user with ID '{userManager.GetUserId(context.User)}'.");
        }

        var userId = await userManager.GetUserIdAsync(user);
        Log.Information($"User with ID '{userId}' asked for their personal data.");

        // Only include personal data for download
        var personalDataProps = typeof(ApplicationUser).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
        var logins = await userManager.GetLoginsAsync(user);

        var personalData = personalDataProps.ToDictionary(p => p.Name, p => p.GetValue(user)?.ToString() ?? "null");
        
        foreach (var l in logins)
        {
            personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
        }

        personalData.Add("Authenticator Key", (await userManager.GetAuthenticatorKeyAsync(user))!);
        
        var fileBytes = JsonSerializer.SerializeToUtf8Bytes(personalData);
        
        context.Response.Headers.TryAdd("Content-Disposition", "attachment; filename=user-data.json");
        
        return TypedResults.File(fileBytes, contentType: "application/json", fileDownloadName: "user-data.json");
    }

    private static async Task<RedirectHttpResult> OnLogout(ClaimsPrincipal _, SignInManager<ApplicationUser> signInManager, [FromForm] string returnUrl)
    {
        await signInManager.SignOutAsync();
        return TypedResults.LocalRedirect($"~/{returnUrl}");
    }
}
