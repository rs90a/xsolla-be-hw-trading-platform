using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Payment
{
    public class SessionInfo
    {
        [Required]
        public string SessionId { get; set; }
        
        [Required]
        public string CardNumber { get; set; }
    }
}