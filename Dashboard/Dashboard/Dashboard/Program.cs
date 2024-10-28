using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Travel.Data;
using Travel.Models;

namespace Dashboard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register DbContext with Identity support
            builder.Services.AddDbContext<TravelDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register Identity services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TravelDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Ensure roles and users are seeded at startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedData.Initialize(services);
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();  // Enable Authentication Middleware
            app.UseAuthorization();   // Enable Authorization Middleware

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
