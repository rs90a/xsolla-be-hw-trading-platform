using TradingPlatform.Models.Payment;

namespace TradingPlatform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса кэширования
    /// </summary>
    public interface ICacheService
    {
        public PaymentInfo GetPaymentInfo(string sessionId);
        public void AddPaymentInfo(string sessionId, PaymentInfoCache paymentInfo);
        public void RemovePaymentInfo(string sessionId);
    }
}