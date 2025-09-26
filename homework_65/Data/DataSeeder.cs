using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyChat.Models;

namespace MyChat.Data
{
    public static class DataSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    var user = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        Avatar = "/images/default.png"
                    };

                    var result = userManager.CreateAsync(user, "Admin123!").Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception("Не удалось создать пользователя для сидера: " +
                                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                var firstUser = context.Users.First();
                if (!context.Messages.Any())
                {
                    context.Messages.Add(new Message
                    {
                        Text = "Привет! Это тестовое сообщение.",
                        SentAt = DateTime.Now,
                        UserId = firstUser.Id
                    });

                    context.Messages.Add(new Message
                    {
                        Text = "Второе сообщение от того же пользователя.",
                        SentAt = DateTime.Now,
                        UserId = firstUser.Id
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}