using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models
{
    /// <summary>
    /// Конфигурация Smtp
    public class SmtpConfig
    {
        [Required]
        public string Host { get; set; }
        
        [Required]
        public int Port { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string SenderEmail { get; set; }
        
        [Required]
        public string SenderPassword { get; set; }
    }
}