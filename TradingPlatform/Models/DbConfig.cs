using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models
{
    /// <summary>
    /// Конфигурация базы данных
    /// </summary>
    public class DbConfig
    {
        [Required]
        public string Host { get; set; }
        
        [Required]
        public int Port { get; set; }
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Database { get; set; }
    }
}