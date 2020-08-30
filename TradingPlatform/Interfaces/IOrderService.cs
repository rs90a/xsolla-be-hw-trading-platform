using System.Threading.Tasks;
using TradingPlatform.Models;
using TradingPlatform.Models.Join;
using TradingPlatform.Models.Payment;

namespace TradingPlatform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с историей заказов
    /// </summary>
    public interface IOrderService
    {
        public Task<OrderInfo> AddOrder(User user, PaymentInfoCache paymentInfoCache, SessionInfo sessionInfo);
    }
}