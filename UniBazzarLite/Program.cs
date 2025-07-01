using UniBazaarLite.Data;
using UniBazaarLite.Filters;
using UniBazaarLite.Middlewares;
using UniBazaarLite.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// ----- DI -----
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<LogActivityFilter>();   // global log
});
builder.Services.AddRazorPages(options =>
{
    // "Events/Index" sayfasýný "/" rotasýna da baðla
    options.Conventions.AddPageRoute("/Events/Index", "/");
});
builder.Services.AddUniBazaarRepositories();

builder.Services.AddScoped<LogActivityFilter>();
builder.Services.AddScoped<ValidateEventExistsFilter>();
builder.Services.AddScoped<ValidateItemExistsFilter>();

builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection("Site"));

// basit fake auth
builder.Services.AddAuthentication("Fake")
    .AddCookie("Fake", o => o.LoginPath = "/");
builder.Services.AddAuthorization();
builder.Services.Configure<SiteOptions>(
    builder.Configuration.GetSection("SiteOptions"));


var app = builder.Build();

// ----- pipeline -----
app.UseMiddleware<CurrentUserMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();   // Razor Pages
app.MapControllers();

app.Run();
