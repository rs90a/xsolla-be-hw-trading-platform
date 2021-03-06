﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TradingPlatform.Models;
using TradingPlatform.Models.Statistics;

namespace TradingPlatform.Database
{
    /// <summary>
    /// Инициализация базовых ролей и пользователей
    /// </summary>
    public class IdentityInitializer
    {
        private readonly IConfiguration configuration;
        private readonly TradingPlatformDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityInitializer(IConfiguration configuration, TradingPlatformDbContext dbContext,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task InitAsync()
        {
            await InitRolesAsync();
            await InitUsersAsync();
            await InitPlatformStatistics();
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
            
            var users = new List<UserConfig>();
            usersSection.Bind(users);

            foreach (var user in users)
            {
                var newUser = new User()
                {
                    UserName = user.Email,
                    Email = user.Email,
                    Roles =  user.Roles
                };
                var result = await userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                    await userManager.AddToRolesAsync(newUser, user.Roles);
            }
        }

        private async Task AddRole(string role)
        {
            if (await roleManager.FindByNameAsync(role) == null)
                await roleManager.CreateAsync(new IdentityRole(role));
        }
        
        private async Task InitPlatformStatistics()
        {
            if (dbContext.PlatformStatistics.Any())
                return;
            
            await dbContext.PlatformStatistics.AddAsync(
                new PlatformStatisticsDto
                {
                    Balance = 0
                }
            );
            await dbContext.SaveChangesAsync();
        }
    }
}