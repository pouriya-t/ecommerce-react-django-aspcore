using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }


            if (!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Id = "1",
                        DisplayName = "Pouriya",
                        UserName = "Admin",
                        Email = "admin@gmail.com"
                    },
                    new User
                    {
                        Id = "2",
                        DisplayName = "User",
                        UserName = "User",
                        Email = "user@gmail.com"
                    },
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Admin123*");
                    if(user.UserName == "Admin")
                    {
                        await userManager.AddToRoleAsync(user, UserRoles.Admin);
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, UserRoles.User);
                    }
                }
            }



            if (!context.Product.Any())
            {
                var products = new List<Product>
                {
                     new Product
                {
                    name = "Airpods Wireless Bluetooth Headphones",
                    image = "/images/airpods.jpg",
                    brand = "Apple",
                    category = "Electronics",
                    description = "ABout Airpods Wireless",
                    rating = 21,
                    numReviews = 4,
                    price = 22,
                    countInStock = 51,
                    createdAt = DateTime.Now,
                    user_id = "1"
                },
                new Product
                {
                    name = "iPhone 11 Pro 256GB Memory",
                    image = "/images/phone.jpg",
                    brand = "Apple",
                    category = "Electronics",
                    description = "ABout this",
                    rating = 41,
                    numReviews = 5,
                    price = 32,
                    countInStock = 5,
                    createdAt = DateTime.Now,
                    user_id = "2"
                }};

                context.Product.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
