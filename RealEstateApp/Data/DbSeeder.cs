using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace RealEstateApp.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Get the required services
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. Create the "Admin" Role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 2. Create the "Agent" Role (optional, for future use)
            if (!await roleManager.RoleExistsAsync("Agent"))
            {
                await roleManager.CreateAsync(new IdentityRole("Agent"));
            }

            // 3. Create a default Admin User
            var adminEmail = "admin@realestate.com"; // CHANGE THIS if you want
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Create user with password "Admin@123"
                var result = await userManager.CreateAsync(newAdmin, "Admin@123");

                if (result.Succeeded)
                {
                    // Assign the "Admin" role to this user
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}