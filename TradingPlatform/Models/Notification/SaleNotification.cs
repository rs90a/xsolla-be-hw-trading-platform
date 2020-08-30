using System.ComponentModel.DataAnnotations;
using TradingPlatform.Models.Join;
using TradingPlatform.Models.Statistics;

namespace TradingPlatform.Models.Notification
{
    public class SaleNotification
    {
        [Required]
        public OrderInfo Order { get; set; }
        
        [Required]
        public Earnings Earnings { get; set; }
    }
}