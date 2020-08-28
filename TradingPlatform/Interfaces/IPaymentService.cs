using System.Threading.Tasks;
using TradingPlatform.Models.Payment;

namespace TradingPlatform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с платежами
    /// </summary>
    public interface IPaymentService
    {
        public Task<string> CreateSession(PaymentInfo paymentInfo);
    }
}