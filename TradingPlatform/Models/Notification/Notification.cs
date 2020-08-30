using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Notification
{
    public class Notification
    {
        [Url]
        public string Url { get; set; }
        
        [Required]
        public object Body { get; set; }
    }
}