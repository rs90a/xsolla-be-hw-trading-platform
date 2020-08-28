using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Payment;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Сервис для работы с платежами
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly ICacheService cache;
        private readonly IKeystoreService keystoreService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PaymentService(ICacheService cache, IKeystoreService keystoreService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.cache = cache;
            this.keystoreService = keystoreService;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Создание сессии
        /// </summary>
        /// <param name="paymentInfo">Сведения о платеже</param>
        /// <returns>Id сессии</returns>
        public async Task<string> CreateSession(PaymentInfo paymentInfo)
        {
            if (!await keystoreService.СheckGameHasKeys(paymentInfo.Game.Id))
                throw new ArgumentException(@$"Игровые ключи для игры ""{paymentInfo.Game.Name}"" закончились");

            var userEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail) || userEmail != paymentInfo.Email)
                throw new ArgumentException("Указанный Email не совпадает с Email в Вашем профиле");

            var sessionId = Guid.NewGuid().ToString();
            cache.AddPaymentInfo(sessionId, paymentInfo);

            return sessionId;
        }
    }
}