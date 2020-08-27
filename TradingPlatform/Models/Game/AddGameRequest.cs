using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Game
{
    /// <summary>
    /// Модель для добавления нового товара
    /// </summary>
    public class AddGameRequest
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public int Price { get; set; }
        
        [Required]
        public List<string> Keys { get; set; }
    }
}