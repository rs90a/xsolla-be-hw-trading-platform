using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using TradingPlatform.Cache;
using TradingPlatform.Database;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Payment;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Сервис кэширования
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache cache;
        private readonly IServiceProvider serviceProvider;

        public CacheService(IMemoryCache cache, IServiceProvider serviceProvider)
        {
            this.cache = cache;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Получение сведений о платежной сессии
        /// </summary>
        /// <param name="sessionId">Id сессии</param>
        /// <returns>Сведения о платеже</returns>
        public PaymentInfoCache GetPaymentInfo(string sessionId)
        {
            PaymentInfoCache paymentInfoCache;
            if (cache.TryGetValue($"{CacheKeys.Session}_{sessionId}", out paymentInfoCache))
                return paymentInfoCache;
            throw new ArgumentException($"Время сессии {sessionId} истекло.");
        }

        /// <summary>
        /// Добавление сведений о платежной сессии в кэш. Время сессии - 2 мин.
        /// </summary>
        /// <param name="sessionId">Id сессии</param>
        /// <param name="paymentInfo">Сведения о платеже</param>
        public void AddPaymentInfo(string sessionId, PaymentInfoCache paymentInfo)
        {
            var expirationMinutes = 2;

            /* Для автоматического удаления записи кэша, по истечению срока годности.
                В следствие чего срабатывает и сам Callback - PostEvictionCallbacks.
                По умолчанию записи из кэша удаляются лениво, т.е. все истекшие записи хранятся в кэше до следующего обращения к кэшу.
            */
            var expirationToken = new CancellationChangeToken(
                new CancellationTokenSource(TimeSpan.FromMinutes(expirationMinutes)).Token);

            cache.Set($"{CacheKeys.Session}_{sessionId}", paymentInfo, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes),
                ExpirationTokens = {expirationToken},
                PostEvictionCallbacks =
                {
                    new PostEvictionCallbackRegistration()
                    {
                        EvictionCallback = DeletingSessionCallback
                    }
                }
            });
        }

        /// <summary>
        /// Удаление сведений о сессии из кэша
        /// </summary>
        /// <param name="sessionId">Id сессии</param>
        public void RemovePaymentInfo(string sessionId)
        {
            cache.Remove($"{CacheKeys.Session}_{sessionId}");
        }

        /// <summary>
        /// Callback-метод, вызываемый при удалении записи о сессии из кэша
        /// </summary>
        private async void DeletingSessionCallback(object key, object value, EvictionReason reason, object state)
        {
            if (value is PaymentInfoCache paymentInfoCache)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TradingPlatformDbContext>();
                    
                    var keyDtoFromDb = await dbContext.Keys.FirstOrDefaultAsync(keyDto => 
                        keyDto.Key == paymentInfoCache.KeyDto.Key && keyDto.GameId == paymentInfoCache.Game.Id);
                
                    //Игра была куплена до истечения платежной сессии
                    if (keyDtoFromDb == null)
                        return;
                
                    keyDtoFromDb.Reserved = false;
                    dbContext.Keys.Update(keyDtoFromDb);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}