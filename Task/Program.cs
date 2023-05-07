using Microsoft.EntityFrameworkCore;
using Task.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Service

builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("Default");
    options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
});

builder.Services.AddControllersWithViews(); // Decleare MVC arch
var app = builder.Build();

// Middleware

app.UseStaticFiles(); // Use static file  (index.html)

// Admin Route
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

// Main Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();

