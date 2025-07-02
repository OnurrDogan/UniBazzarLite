using UniBazaarLite.Data;
using UniBazaarLite.Filters;
using UniBazaarLite.Middlewares;
using UniBazaarLite.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// ----- Dependency Injection (DI) setup -----
// Add MVC controllers with global logging filter
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<LogActivityFilter>(); // For MVC requests
});
// Add Razor Pages with global logging filter and custom routing
builder.Services.AddRazorPages(options =>
{
    // Redirect root URL to events index
    options.Conventions.AddPageRoute("/Events/Index", "/");
})
.AddMvcOptions(o =>
{
    o.Filters.Add<LogActivityFilter>(); // For Razor Pages requests too
});
builder.Services.AddUniBazaarRepositories(); // Register our in-memory repositories

// Register filters for DI
builder.Services.AddScoped<LogActivityFilter>();
builder.Services.AddScoped<ValidateEventExistsFilter>();
builder.Services.AddScoped<ValidateItemExistsFilter>();

// Load site options from config
builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection("Site"));

// Add AdminEmail from User Secrets (final project requirement)
builder.Configuration.AddUserSecrets<Program>();

// Simulated/fake authentication setup
builder.Services.AddAuthentication("Fake")
    .AddCookie("Fake", o => o.LoginPath = "/");
builder.Services.AddAuthorization();
builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection("Site"));

var app = builder.Build();

// ----- HTTP request pipeline setup -----
app.UseMiddleware<CurrentUserMiddleware>(); // Simulate a logged-in user
app.UseStaticFiles(); // Serve static files (css, js, etc.)
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();   // Razor Pages
app.MapControllers(); // MVC Controllers

app.Run();
