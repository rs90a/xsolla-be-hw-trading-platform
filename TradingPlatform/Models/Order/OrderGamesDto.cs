using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Order
{
    public class OrderGamesDto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public double Price { get; set; }
    }
}