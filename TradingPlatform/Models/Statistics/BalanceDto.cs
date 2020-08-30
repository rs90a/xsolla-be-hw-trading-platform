using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Statistics
{
    public class BalanceDto
    {
        [Key]
        public string UserId { get; set; }
        
        [Required]
        public double Value { get; set; }
    }
}