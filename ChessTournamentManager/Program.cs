using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChessTournamentManager;
using ChessTournamentManager.Endpoints;
using ChessTournamentManager.Services;
using ChessTournamentManager.Core.User;
using ChessTournamentManager.Infra;
using ChessTournamentManager.Infra.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logging

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

#endregion

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, action => action.MigrationsAssembly("ChessTournamentManager.Infra")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#region Authentication and Authorization

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = true;
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();
builder.Services
    .AddScoped<IUserEmailStore<ApplicationUser>,
        UserStore<ApplicationUser, IdentityRole<Guid>, ApplicationDbContext, Guid>>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"))
    .AddPolicy("RequireOrganizerRole", policy => policy.RequireRole("Organizer"))
    .AddPolicy("RequirePlayerRole", policy => policy.RequireRole("Player"))
    .AddPolicy("RequireRefereeRole", policy => policy.RequireRole("Referee"))
    .AddPolicy(Permissions.GeneratePermission(Module.Tournaments, Permission.Create),
        policy => policy.AddPermissions(Module.Tournaments, Permission.Create))
    .AddPolicy(Permissions.GeneratePermission(Module.Tournaments, Permission.Read),
        policy => policy.AddPermissions(Module.Tournaments, Permission.Read))
    .AddPolicy(Permissions.GeneratePermission(Module.Tournaments, Permission.Update),
        policy => policy.AddPermissions(Module.Tournaments, Permission.Update))
    .AddPolicy(Permissions.GeneratePermission(Module.Tournaments, Permission.Delete),
        policy => policy.AddPermissions(Module.Tournaments, Permission.Delete))
    .AddPolicy(Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Create),
        policy => policy.AddPermissions(Module.TournamentPlayers, Permission.Create))
    .AddPolicy(Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Read),
        policy => policy.AddPermissions(Module.TournamentPlayers, Permission.Read))
    .AddPolicy(Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Update),
        policy => policy.AddPermissions(Module.TournamentPlayers, Permission.Update))
    .AddPolicy(Permissions.GeneratePermission(Module.TournamentPlayers, Permission.Delete),
        policy => policy.AddPermissions(Module.TournamentPlayers, Permission.Delete))
    .AddPolicy(Permissions.GeneratePermission(Module.Games, Permission.Create),
        policy => policy.AddPermissions(Module.Games, Permission.Create))
    .AddPolicy(Permissions.GeneratePermission(Module.Games, Permission.Read),
        policy => policy.AddPermissions(Module.Games, Permission.Read))
    .AddPolicy(Permissions.GeneratePermission(Module.Games, Permission.Update),
        policy => policy.AddPermissions(Module.Games, Permission.Update))
    .AddPolicy(Permissions.GeneratePermission(Module.Games, Permission.Delete),
        policy => policy.AddPermissions(Module.Games, Permission.Delete))
    .AddPolicy(Permissions.GeneratePermission(Module.Results, Permission.Create),
        policy => policy.AddPermissions(Module.Results, Permission.Create))
    .AddPolicy(Permissions.GeneratePermission(Module.Results, Permission.Read),
        policy => policy.AddPermissions(Module.Results, Permission.Read))
    .AddPolicy(Permissions.GeneratePermission(Module.Results, Permission.Update),
        policy => policy.AddPermissions(Module.Results, Permission.Update))
    .AddPolicy(Permissions.GeneratePermission(Module.Results, Permission.Delete),
        policy => policy.AddPermissions(Module.Results, Permission.Delete));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

#endregion

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddMudServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An error occurred while migrating or initializing the database.");
        throw;
    }
    
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    
    try
    {
        await roleManager.SeedAsync();
        await roleManager.SeedClaimsAsync();
        await userManager.SeedAsync();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An error occurred while seeding the database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ChessTournamentManager.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();

Log.CloseAndFlush();