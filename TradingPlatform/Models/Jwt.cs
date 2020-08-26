using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models
{
    /// <summary>
    /// Класс с параметрами JWT-конфигурации
    /// </summary>
    public class Jwt
    {
        [Required]
        public string Issuer { get; set; }
        
        [Required]
        public string Audience { get; set; }
        
        [Required]
        public string Key { get; set; }
        
        //Время жизни токена в минутах
        [Required]
        public int LifeTime { get; set; }
    }
}