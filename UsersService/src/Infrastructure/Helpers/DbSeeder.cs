using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Helpers
{
    public class DbSeeder
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var userRoleName = configuration.GetValue<string>("SeedinDbParams:UserRoleName");
            var adminRoleName = configuration.GetValue<string>("SeedinDbParams:AdminRoleName");

            if (await roleManager.FindByNameAsync(userRoleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(userRoleName));
            }
            if (await roleManager.FindByNameAsync(adminRoleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            }

            string adminEmail = configuration.GetValue<string>("SeedinDbParams:AdminAccEmail");
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                string adminPwd = configuration.GetValue<string>("SeedinDbParams:AdminAccPwd");
                User admin = new User { Email = adminEmail, UserName = configuration.GetValue<string>("SeedinDbParams:AdminUserName") };
                IdentityResult result = await userManager.CreateAsync(admin, adminPwd);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminRoleName);
                }
            }

            if ((await userManager.GetUsersInRoleAsync(userRoleName)).Count == 0)
            {
                User user = new User { UserName = "Useeer1" };
                var userPwd = configuration.GetValue<string>("SeedinDbParams:FirstUserAccPwd");

                IdentityResult result = await userManager.CreateAsync(user, userPwd);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userRoleName);
                }
            }
        }
    }
}
