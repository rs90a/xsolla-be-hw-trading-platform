using System.Threading.Tasks;
using TradingPlatform.Models.Payment;

namespace TradingPlatform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с Smtp
    /// </summary>
    public interface ISmtpService
    {
        public Task SendPurchaseNotification(PaymentInfoCache paymentInfoCache);
    }
}