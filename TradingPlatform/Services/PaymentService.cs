using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TradingPlatform.Database;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Payment;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Сервис для работы с платежами
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private const double CommissionPercentage = 5;

        private readonly TradingPlatformDbContext dbContext;
        private readonly ICacheService cache;
        private readonly IKeystoreService keystoreService;
        private readonly IOrderService orderService;
        private readonly IAccountService accountService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISmtpService smtpService;
        
        public PaymentService(TradingPlatformDbContext dbContext, ICacheService cache, IKeystoreService keystoreService, 
            IAccountService accountService, IOrderService orderService, IHttpContextAccessor httpContextAccessor, ISmtpService smtpService)
        {
            this.dbContext = dbContext;
            this.cache = cache;
            this.keystoreService = keystoreService;
            this.accountService = accountService;
            this.orderService = orderService;
            this.httpContextAccessor = httpContextAccessor;
            this.smtpService = smtpService;
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
            var user = await accountService.GetCurrentUser();
            await orderService.AddOrder(
                user,  
                paymentInfoCache, 
                new SessionInfo()
                {
                    CardNumber = paymentByCard.Card.Number,
                    SessionId = paymentByCard.SessionId
                });
            await DistributeMoney(paymentInfoCache);
            await keystoreService.DeleteKey(paymentInfoCache.KeyDto.Id, paymentInfoCache.Game.Id);
            cache.RemovePaymentInfo(paymentByCard.SessionId);
            await smtpService.SendPurchaseNotification(paymentInfoCache);
        }

        /// <summary>
        /// Распределение денежных средств между плафтормой и продацом
        /// </summary>
        private async Task DistributeMoney(PaymentInfoCache paymentInfoCache)
        {
            var commission = paymentInfoCache.Amount / 100 * CommissionPercentage;

            var sellerBalance = dbContext.Balances.FirstOrDefault(balance => 
                balance.UserId == paymentInfoCache.Game.SellerId);
            var platformStats = dbContext.PlatformStatistics.FirstOrDefault();

            if (sellerBalance == null || platformStats == null)
                throw new ArgumentException("Не удалось выполнить пополнение баланса");

            sellerBalance.Value += paymentInfoCache.Amount - commission;
            platformStats.Balance += commission;
            
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                dbContext.Balances.Update(sellerBalance);
                dbContext.PlatformStatistics.Update(platformStats);
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }

        }
    }
}