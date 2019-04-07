using Mes.Core.Domain;
using Mes.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mes.API.Configuration
{
    public class ConfigurationDbContextSeed
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var identityContext = scope.ServiceProvider.GetRequiredService<MesDbContext>();
                identityContext.Database.Migrate();
                if (!identityContext.Users.Any())
                {
                    if (!identityContext.Roles.Any())
                    {

                        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                        roleManager.CreateAsync(new ApplicationRole { Name = "admin", Id = Guid.NewGuid().ToString(), DisplayName = "系统管理员" })
                           .ConfigureAwait(false).GetAwaiter().GetResult();
                        roleManager.CreateAsync(new ApplicationRole { Name = "superadmin", Id = Guid.NewGuid().ToString(), DisplayName = "超级管理员" })
                         .ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    if (!identityContext.Users.Any())
                    {
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var user = new ApplicationUser()
                        {
                            Email = "admin@gmail.com",
                            PhoneNumber = "15996586015",
                            UserName = "admin",
                            NickName = "Admin",
                            PhoneNumberConfirmed = true,
                            EmailConfirmed = true,
                        };
                        userManager.CreateAsync(user, "test@123").ConfigureAwait(false).GetAwaiter().GetResult();
                        userManager.AddToRolesAsync(user, new string[] { "admin", "superadmin" }).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                }
            }
        }
    }
}
