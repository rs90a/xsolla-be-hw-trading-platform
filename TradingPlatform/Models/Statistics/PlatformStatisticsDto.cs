using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Statistics
{
    public class PlatformStatisticsDto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public double Balance { get; set; }
    }
}