using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Game
{
    /// <summary>
    /// Модель с информацией об игре
    /// </summary>
    public class PaymentGameRequest
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string SellerId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}