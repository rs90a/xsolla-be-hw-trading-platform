using System.ComponentModel.DataAnnotations;
using TradingPlatform.Validators;

namespace TradingPlatform.Models.Payment
{
    public class PaymentByCard
    {
        [Required]
        [CardValidator]
        public Card Card { get; set; }
        
        [Required]
        public string SessionId { get; set; }
    }
}