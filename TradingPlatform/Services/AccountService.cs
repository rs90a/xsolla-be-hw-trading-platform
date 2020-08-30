using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TradingPlatform.Database;
using TradingPlatform.Enum;
using TradingPlatform.Interfaces;
using TradingPlatform.Models;
using TradingPlatform.Models.Statistics;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Cервис для работы c пользователями торговой площадки
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IAuth authService;
        private readonly UserManager<User> userManager;
        private readonly TradingPlatformDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        
        public AccountService(IAuth authService, UserManager<User> userManager, 
            TradingPlatformDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.authService = authService;
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Получение токена
        /// </summary>
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

        /// <summary>
        /// Получение Id текущго  пользователя
        /// </summary>
        public string GetCurrentUserId()
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("Пользователь не найден");

            return userId;
        }

        /// <summary>
        /// Получение текущго пользователя
        /// </summary>
        public async Task<User> GetCurrentUser()
        {
            var userEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentException("Email пользователя не найден");
            
            return await userManager.FindByEmailAsync(userEmail);
        }

        /// <summary>
        /// Получение пользователя по Id
        /// </summary>
        public async Task<User> GetUserById(string userId) =>
            await userManager.FindByIdAsync(userId);


        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        public async Task<IdentityResult> AddUser(SignUp signUp)
        {
            var result = await userManager.CreateAsync(
                new User
                {
                    Email = signUp.Email,
                    UserName = signUp.Email,
                    Roles = new List<string>() {signUp.Role.ToString().ToLower()},
                    NotificationUrl = signUp.Role == Roles.Seller ? signUp.NotificationUrl : ""
                },
                signUp.Password);

            await InitBalance(signUp);
            
            return result;
        }

        /// <summary>
        /// Инициализация баланса нового продавца
        /// </summary>
        private async Task InitBalance(SignUp signUp)
        {
            if (signUp.Role == Roles.Seller)
            {
                await dbContext.Balances.AddAsync(new BalanceDto
                {
                    UserId = (await userManager.FindByEmailAsync(signUp.Email)).Id,
                    Value = 0
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}