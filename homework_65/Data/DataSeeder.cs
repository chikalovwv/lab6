using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using MyChat.Models;

namespace MyChat.Data
{
    public static class DataSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!roleManager.RoleExistsAsync("admin").Result)
            {
                IdentityRole adminRole = new IdentityRole("admin");
                roleManager.CreateAsync(adminRole).Wait();
            }

            if (!roleManager.RoleExistsAsync("user").Result)
            {
                IdentityRole userRole = new IdentityRole("user");
                roleManager.CreateAsync(userRole).Wait();
            }

            ApplicationUser admin = userManager.FindByNameAsync("admin").Result;
            if (admin == null)
            {
                ApplicationUser newAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    BirthDate = new System.DateTime(1990,1,1),
                    AvatarUrl = "/images/default-avatar.png",
                    EmailConfirmed = true
                };
                IdentityResult result = userManager.CreateAsync(newAdmin, "Admin1!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(newAdmin, "admin").Wait();
                }
            }
            ApplicationUser user1 = userManager.FindByNameAsync("user1").Result;
            if (user1 == null)
            {
                ApplicationUser u1 = new ApplicationUser
                {
                    UserName = "user1",
                    Email = "user1@example.com",
                    BirthDate = new System.DateTime(1995,1,1),
                    AvatarUrl = "/images/default-avatar.png",
                    EmailConfirmed = true
                };
                IdentityResult r = userManager.CreateAsync(u1, "User1!a").Result;
                if (r.Succeeded)
                {
                    userManager.AddToRoleAsync(u1, "user").Wait();
                }
            }
            if (!context.Messages.Any())
            {
                Message m1 = new Message
                {
                    Id = 1,
                    UserName = "user1",
                    Text = "Привет! Это тестовое сообщение.",
                    SentAt = DateTime.UtcNow.AddMinutes(-10),
                    AvatarUrl = "/images/default-avatar.png"
                };
                Message m2 = new Message
                {
                    Id = 2,
                    UserName = "admin",
                    Text = "Добро пожаловать в чат MyChat!",
                    SentAt = DateTime.UtcNow.AddMinutes(-5),
                    AvatarUrl = "/images/default-avatar.png"
                };

                context.Messages.Add(m1);
                context.Messages.Add(m2);
                context.SaveChanges();
            }
        }
    }
}