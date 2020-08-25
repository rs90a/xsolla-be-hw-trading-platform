using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TradingPlatform.Models;

namespace TradingPlatform.Database
{
    /// <summary>
    /// Инициализация базовых ролей и пользователей
    /// </summary>
    public class IdentityInitializer
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        
        public IdentityInitializer(IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task InitAsync()
        {
            await InitRolesAsync();
            await InitUsersAsync();
        }

        private async Task InitRolesAsync()
        {
            var rolesSection = configuration.GetSection("Roles");
            if (rolesSection == null)
                return;

            var roles = new List<string>();
            rolesSection.Bind(roles);

            foreach (var role in roles)
                await AddRole(role);
        }

        private async Task InitUsersAsync()
        {
            var usersSection = configuration.GetSection("Users");
            if (usersSection == null)
                return;
            
            var users = new List<User>();
            usersSection.Bind(users);

            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                    await userManager.AddToRolesAsync(user, user.Roles);
            }
        }

        private async Task AddRole(string role)
        {
            if (await roleManager.FindByNameAsync(role) == null)
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}