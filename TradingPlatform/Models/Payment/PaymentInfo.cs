using System.ComponentModel.DataAnnotations;
using TradingPlatform.Models.Game;

namespace TradingPlatform.Models.Payment
{
    /// <summary>
    /// Сведения о платеже
    /// </summary>
    public class PaymentInfo
    {
        [Required]
        [Range(1, 1000000, ErrorMessage = "Размер платежа должен быть от 1 до 1000000 у.е.")]
        public double Amount { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        public PaymentGameRequest Game { get; set; }
    }
}