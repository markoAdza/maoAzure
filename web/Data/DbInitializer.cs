using web.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MaoContext context)
        {
            context.Database.EnsureCreated();

            if (context.Menus.Any())
            {
                return;   // DB has been seeded
            }

            // Add roles

            var roles = new IdentityRole[] {
                new IdentityRole{Id="1", Name="Administrator"},
                new IdentityRole{Id="2", Name="Client"}
            };

            foreach (IdentityRole r in roles)
            {
                context.Roles.Add(r);
            }

            context.SaveChanges();


            // Add user janez@example.com (ADMIN + MANAGER)

            var user = new ApplicationUser
            {
                FirstName = "Janez",
                LastName = "Novak",
                City = "Ljubljana",
                Email = "janez@example.com",
                NormalizedEmail = "XXXX@EXAMPLE>.COM",
                UserName = "janez@example.com",
                NormalizedUserName = "janez@example.com",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "Testni123!");
                user.PasswordHash = hashed;
                context.Users.Add(user);
            }

            context.SaveChanges();


            var UserRoles = new IdentityUserRole<string>[] {
                new IdentityUserRole<string>{RoleId = roles[0].Id, UserId = user.Id},
                new IdentityUserRole<string>{RoleId = roles[1].Id, UserId = user.Id}
            };

            foreach (IdentityUserRole<string> r in UserRoles)
            {
                context.UserRoles.Add(r);
            }

            context.SaveChanges();


            // Add menu items

            var menus = new Menu[]
            {
                new Menu {MenuType = "meat", FoodType="main", FoodName = "Chicken Chopsui"},
                new Menu {MenuType = "meat", FoodType="main", FoodName = "Beef Chopsui"},
                new Menu {MenuType = "vegan", FoodType="main", FoodName = "Vegan Noodles"},

                new Menu {MenuType = "meat", FoodType="soup", FoodName = "Sweet Spicy Soup"},
                new Menu {MenuType = "meat", FoodType="soup", FoodName = "Vegan Sweet Spicy Soup"},

                new Menu {MenuType = "no alergens", FoodType="dessert", FoodName = "Fried Bananas"},
                new Menu {MenuType = "no alergens", FoodType="dessert", FoodName = "Fried Apples"},
            };

            foreach (Menu m in menus)
            {
                context.Menus.Add(m);
            }
            context.SaveChanges();



        }
    }
}