// This is EF Core
using ConfuciusWebsite.Data;
using ConfuciusWebsite.Models;
using ConfuciusWebsite.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Getting the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Setting up the database context to use SQL Server with the provided connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register ASP.NET Core Identity Services using AddIdentity
builder.Services.AddIdentity<AdminUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Adding support for controllers with views
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

// Adding session support
builder.Services.AddSession();

// Registering the ImageService for dependency injection
builder.Services.AddScoped<IImageService, ImageService>();




// Building the application
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    if (app.Environment.IsDevelopment())
    {
    
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AdminUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // Ensure roles exist
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));

        if (!await roleManager.RoleExistsAsync("Guest"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("Guest"));

        // Create admin user if not exists
        var adminEmail = "admin@example.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new AdminUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, "Admin123!"); // choose a strong password
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Enables session state for the application
/*
 * The Authorization and Authentication lines are placed after the routing middleware (app.UseRouting()) 
 * but before the MVC routing middleware (app.MapControllerRoute(…)) 
 * so that the Identity system can authenticate and authorize users before requests reach my controllers.
 */
app.UseAuthentication(); // Enables the authentication system to validate user credentials
app.UseAuthorization(); // Enables authorization checks for access control based on roles or policies

// Setting up area routing for Admin area
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Setting up default routing /Public/
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        if (feature?.Error is BadHttpRequestException badReq &&
            badReq.Message.Contains("Request body too large"))
        {
            // Try to preserve route values (like id, area, controller, action)
            var routeValues = context.Request.RouteValues;

            var area = routeValues["area"]?.ToString();
            var controller = routeValues["controller"]?.ToString();
            var action = routeValues["action"]?.ToString();
            var id = routeValues["id"]?.ToString();

            // Build redirect URL back to the same action with a flag
            var url = "";

            if (!string.IsNullOrEmpty(area))
                url += $"/{area}";

            if (!string.IsNullOrEmpty(controller))
                url += $"/{controller}";

            if (!string.IsNullOrEmpty(action))
                url += $"/{action}";

            if (!string.IsNullOrEmpty(id))
                url += $"/{id}";

            // Add query flag
            url += (url.Contains("?") ? "&" : "?") + "fileTooLarge=1";

            context.Response.Redirect(url);
            return;
        }

        // Fallback for other exceptions
        context.Response.Redirect("/Error");
    });
});


app.Run();
