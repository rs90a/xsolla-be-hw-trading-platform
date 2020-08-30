using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Notification;

namespace TradingPlatform.Services.Notifier
{
    /// <summary>
    /// Сервис уведомлений
    /// </summary>
    public class NotifierService : INotifierService
    {
        private readonly HttpClient httpClient;

        public NotifierService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task SendNotification(Notification notification)
        {
            if (string.IsNullOrEmpty(notification.Url))
                return;
            
            var content = new StringContent(
                JsonConvert.SerializeObject(new {notification = notification.Body}),
                Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync(notification.Url, content);

            Console.WriteLine(httpResponse.IsSuccessStatusCode
                ? "Уведомление о продаже отправлено продавцу"
                : "Не удалось отправить уведомление продацу");
        }
    }
}