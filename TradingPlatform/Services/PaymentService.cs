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
            var userEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail) || userEmail != paymentInfo.Email)
                throw new ArgumentException("Указанный Email не совпадает с Email в Вашем профиле");

            var reservedKey = await keystoreService.ReserveKey(paymentInfo.Game);
            
            var sessionId = Guid.NewGuid().ToString(); 
            cache.AddPaymentInfo(sessionId, new PaymentInfoCache(paymentInfo, reservedKey));

            return sessionId;
        }

        /// <summary>
        /// Выполнение платежа
        /// </summary>
        /// <param name="paymentByCard">Сведения о карте и сессии</param>
        public async Task BillPayment(PaymentByCard paymentByCard)
        {
            var paymentInfoCache = cache.GetPaymentInfo(paymentByCard.SessionId);
            await keystoreService.DeleteKey(paymentInfoCache.KeyDto.Id, paymentInfoCache.Game.Id);
            cache.RemovePaymentInfo(paymentByCard.SessionId);
        }
    }
}