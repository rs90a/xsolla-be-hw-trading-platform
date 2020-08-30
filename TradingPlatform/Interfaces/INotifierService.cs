using System.Threading.Tasks;
using TradingPlatform.Models.Notification;

namespace TradingPlatform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса уведомлений
    /// </summary>
    public interface INotifierService
    {
        public Task SendNotification(Notification notification);
    }
}