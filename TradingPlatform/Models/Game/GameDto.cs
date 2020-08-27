using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Game
{
    public class GameDto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string SellerId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public int Price { get; set; }
    }
}