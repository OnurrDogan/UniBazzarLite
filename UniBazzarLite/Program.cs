using UniBazzarLite.Data;
using UniBazzarLite.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddUniBazaarRepositories();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<LogActivityFilter>();
builder.Services.AddScoped<ValidateEventExistsFilter>();
builder.Services.AddScoped<ValidateItemExistsFilter>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<LogActivityFilter>();
});
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Events/Index", "/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
