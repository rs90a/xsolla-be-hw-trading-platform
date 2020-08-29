using System.Threading.Tasks;
using MimeKit;
using TradingPlatform.Interfaces;
using TradingPlatform.Models;
using TradingPlatform.Models.Payment;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Cервис для работы c Smtp
    /// </summary>
    public class SmtpService : ISmtpService
    {
        private readonly SmtpConfig smtpConfig;

        public SmtpService(SmtpConfig smtpConfig)
        {
            this.smtpConfig = smtpConfig;
        }

        /// <summary>
        /// Отправка уведомления о покупке игрового ключа
        /// </summary>
        public async Task SendPurchaseNotification(PaymentInfoCache paymentInfoCache) =>
            await SendMessage(paymentInfoCache.Email, 
                "Уведомление о покупке", 
                $"Вы приобрели игровой ключ для игры {paymentInfoCache.Game.Name}. Ваш игровой ключ: {paymentInfoCache.KeyDto.Key}. Спасибо за покупку!");
        
        /// <summary>
        /// Отправка сообщения на Email
        /// </summary>
        private async Task SendMessage(string recipientEmail, string subjectMsg, string bodyMsg)
        {
            var emailMessage = new MimeMessage
            {
                From = {new MailboxAddress("Support GameKeyStore", smtpConfig.SenderEmail)},
                To = {new MailboxAddress("", recipientEmail)},
                ReplyTo = {new MailboxAddress("", smtpConfig.SenderEmail)},
                Subject = subjectMsg,
                Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = bodyMsg
                }
            };
            
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpConfig.Host, smtpConfig.Port, true);
                await client.AuthenticateAsync(smtpConfig.SenderEmail, smtpConfig.SenderPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}