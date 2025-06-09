using System.Security.Claims;
using App.Domain.EF.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.DataSeeding;

public static class AppDataInit
{
    public static void SeedAppData(AppDbContext context)
    {
    }

    public static void MigrateDatabase(AppDbContext context)
    {
        context.Database.Migrate();
    }

    public static void DeleteDatabase(AppDbContext context)
    {
        context.Database.EnsureDeleted();
    }

    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        foreach (var (roleName, id) in InitialData.Roles)
        {
            var role = roleManager.FindByNameAsync(roleName).Result;

            if (role != null) continue;

            role = new AppRole()
            {
                Id = id ?? Guid.NewGuid(),
                Name = roleName,
            };

            var result = roleManager.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException("Role creation failed!");
            }
        }


        foreach (var userInfo in InitialData.Users)
        {
            var user = userManager.FindByEmailAsync(userInfo.name).Result;
            if (user == null)
            {
                user = new AppUser()
                {
                    Id = userInfo.id ?? Guid.NewGuid(),
                    Email = userInfo.name,
                    UserName = userInfo.name,
                    EmailConfirmed = true,
                };
                var result = userManager.CreateAsync(user, userInfo.password).Result;
                if (!result.Succeeded)
                {
                    throw new ApplicationException("User creation failed!");
                }

                result = userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email)).Result;
                if (!result.Succeeded)
                {
                    throw new ApplicationException("Claim adding failed!");
                }
                
            }

            foreach (var role in userInfo.roles)
            {
                if (userManager.IsInRoleAsync(user, role).Result)
                {
                    Console.WriteLine($"User {user.UserName} already in role {role}");
                    continue;
                }

                var roleResult = userManager.AddToRoleAsync(user, role).Result;
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
                else
                {
                    Console.WriteLine($"User {user.UserName} added to role {role}");
                }
            }
        }
    }

    public static void SeedCountries(AppDbContext context)
    {
        foreach (var country in InitialData.Countries)
        {
            var entity = new App.Domain.EF.Destination()
            {
                Id = country.id,
                CountryName = country.CountryName,
                CreatedBy = "admin",
                ChangedBy = null,
                ChangedAt = null,
                CreatedAt = DateTime.UtcNow,
                SysNotes = null,
            };
            context.Destinations.Add(entity);
        }
        var data = context.SaveChanges();
        Console.WriteLine($"Exercise Catgorys added: {data}");
    }
    public static void SeedCurrencys(AppDbContext context)
    {
        foreach (var curr in InitialData.Currencies)
        {
            var entity = new App.Domain.EF.Currency()
            {
                Id = curr.id,
                Code = curr.code,
                Name = curr.Name,
                Rate = curr.Rate,
                CreatedBy = "admin",
                ChangedBy = null,
                ChangedAt = null,
                CreatedAt = DateTime.UtcNow,
                SysNotes = null,
            };
            context.Currencies.Add(entity);
        }
        var data = context.SaveChanges();
        Console.WriteLine($"Exercise Catgorys added: {data}");
    }
    public static void SeedCategories(AppDbContext context)
    {
        foreach (var country in InitialData.Categorys)
        {
            var entity = new App.Domain.EF.ExpenseCategory()
            {
                Id = country.id,
                CategoryName = country.CategoryName,
                CreatedBy = "admin",
                ChangedBy = null,
                ChangedAt = null,
                CreatedAt = DateTime.UtcNow,
                SysNotes = null,
            };
            context.ExpenseCategorys.Add(entity);
        }
        var data = context.SaveChanges();
        Console.WriteLine($"Exercise Catgorys added: {data}");
    }
    public static void SeedSubCategories(AppDbContext context)
    {
        foreach (var country in InitialData.SubCategorys)
        {
            var entity = new App.Domain.EF.ExpenseSubCategory()
            {
                Id = country.id,
                Name = country.Name,
                ExpenseCategoryId = country.categoryId,
                CreatedBy = "admin",
                ChangedBy = null,
                ChangedAt = null,
                CreatedAt = DateTime.UtcNow,
                SysNotes = null,
            };
            context.ExpenseSubCategorys.Add(entity);
        }
        var data = context.SaveChanges();
        Console.WriteLine($"Exercise Catgorys added: {data}");
    }
}
