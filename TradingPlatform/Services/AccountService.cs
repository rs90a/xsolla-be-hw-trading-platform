using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TradingPlatform.Interfaces;
using TradingPlatform.Models;

namespace TradingPlatform.Services
{
    public class AccountService: IAccountService
    {
        private readonly IAuth authService;
        private readonly UserManager<User> userManager;

        public AccountService(IAuth authService, UserManager<User> userManager)
        {
            this.authService = authService;
            this.userManager = userManager;
        }

        public async Task<string> GetToken(SignIn signIn)
        {
            var user = await userManager.FindByEmailAsync(signIn.Email);
            if (user == null)
                throw new ArgumentException(@$"Пользователь с таким email не существует");
            
            var isCorrectPwd = await userManager.CheckPasswordAsync(user, signIn.Password);
            if (!isCorrectPwd)
                throw new ArgumentException("Неверный логин или пароль");
            
            return authService.CreateToken(user);
        }
    }
}