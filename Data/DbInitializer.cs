
using EurasiaBus.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace EurasiaBus.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var roleManager =
            scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // =========================================================
        // СОЗДАНИЕ БАЗЫ ДАННЫХ
        // =========================================================

        await context.Database.MigrateAsync();


        // =========================================================
        // СОЗДАНИЕ РОЛИ ADMIN
        // =========================================================

        const string adminRole = "Admin";

        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            var roleResult =
                await roleManager.CreateAsync(
                    new IdentityRole(adminRole));

            if (!roleResult.Succeeded)
            {
                throw new Exception(
                    "Не удалось создать роль Admin: " +
                    string.Join(", ",
                        roleResult.Errors.Select(x => x.Description)));
            }
        }


        // =========================================================
        // ДАННЫЕ АДМИНИСТРАТОРА
        // =========================================================

        const string adminEmail = "admin@eurasiabus.com";
        const string adminPassword = "Admin123!";


        // =========================================================
        // ПОИСК АДМИНИСТРАТОРА
        // =========================================================

        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);


        // =========================================================
        // СОЗДАНИЕ АДМИНИСТРАТОРА
        // =========================================================

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult =
                await userManager.CreateAsync(
                    adminUser,
                    adminPassword);

            if (!createResult.Succeeded)
            {
                throw new Exception(
                    "Не удалось создать администратора: " +
                    string.Join(", ",
                        createResult.Errors.Select(x => x.Description)));
            }
        }


        // =========================================================
        // НАЗНАЧЕНИЕ РОЛИ ADMIN
        // =========================================================

        if (!await userManager.IsInRoleAsync(
                adminUser,
                adminRole))
        {
            var roleResult =
                await userManager.AddToRoleAsync(
                    adminUser,
                    adminRole);

            if (!roleResult.Succeeded)
            {
                throw new Exception(
                    "Не удалось назначить роль Admin: " +
                    string.Join(", ",
                        roleResult.Errors.Select(x => x.Description)));
            }
        }
    }
}
