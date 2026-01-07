using EPS.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace EPS.Infrastructure.Data;

/// <summary>
/// Seeds ESSENTIAL system data that is ALWAYS required (Roles)
/// Also seeds DEBUG users (admin, HR) in DEBUG mode only
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Seeds roles - ALWAYS runs in ALL environments (Production, Development, etc.)
    /// Roles are required for authorization to work
    /// </summary>
    public static async Task SeedEssentialDataAsync(RoleManager<IdentityRole> roleManager)
    {
        // Seed Roles (CRITICAL - Always needed)
        var roles = UserRole.GetAllRoles();

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    /// <summary>
    /// Seeds DEBUG users (admin, HR) - ONLY in DEBUG mode
    /// This allows immediate testing without manual user creation
    /// NOT included in production builds
    /// </summary>
#if DEBUG
    public static async Task SeedDebugUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Ensure roles exist first
        await SeedEssentialDataAsync(roleManager);

        // Seed Admin User (DEBUG ONLY)
        var adminEmail = "admin@eps.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, UserRole.Admin);
            }
        }

        // Seed HR Manager User (DEBUG ONLY)
        var hrEmail = "hr@eps.com";
        if (await userManager.FindByEmailAsync(hrEmail) == null)
        {
            var hrUser = new ApplicationUser
            {
                UserName = hrEmail,
                Email = hrEmail,
                FullName = "HR Manager",
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(hrUser, "HrManager@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(hrUser, UserRole.HRManager);
            }
        }
    }
#endif
}