using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Game
{
    public class AddGameRequest
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public int Price { get; set; }
    }
}