using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Statistics
{
    public class Earnings
    {
        [Required]
        public double Commission { get; set; }
        
        [Required]
        public double Income { get; set; }
    }
}