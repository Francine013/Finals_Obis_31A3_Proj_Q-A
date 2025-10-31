using Microsoft.AspNetCore.Identity;
using QABoard.Infrastructure.Entities;

namespace QABoard.Infrastructure.Data
{
    public class DbSeeder
    {
        public static async Task SeedData(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            // Create roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Create admin
            if (await userManager.FindByEmailAsync("admin@qaboard.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@qaboard.com",
                    Email = "admin@qaboard.com",
                    FullName = "System Admin",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin@123!!!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Create regular user
            if (await userManager.FindByEmailAsync("user@qaboard.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user@qaboard.com",
                    Email = "user@qaboard.com",
                    FullName = "John Doe",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "User@123!!!");
                await userManager.AddToRoleAsync(user, "User");
            }

            await context.SaveChangesAsync();
        }
    }
}