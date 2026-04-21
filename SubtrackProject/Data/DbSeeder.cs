using Microsoft.AspNetCore.Identity;
using SubtrackProject.Models;

namespace SubtrackProject.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        // 1. Roles
        string[] roles = ["Admin", "User"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // 2. Default admin
        const string adminEmail = "admin@subtrack.com";
        const string adminPassword = "admin123";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                FullName = "Administrator",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        // 3. Default categories (only if table is empty)
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Entertainment", Description = "Streaming, games, media", Color = "#e74c3c" },
                new Category { Name = "Productivity",  Description = "Work, notes, office apps",  Color = "#3498db" },
                new Category { Name = "Education",     Description = "Courses, books, learning",  Color = "#2ecc71" },
                new Category { Name = "Health",        Description = "Fitness, wellness apps",    Color = "#9b59b6" },
                new Category { Name = "Other",         Description = "Uncategorized",             Color = "#95a5a6" }
            );

            await context.SaveChangesAsync();
        }
    }
}