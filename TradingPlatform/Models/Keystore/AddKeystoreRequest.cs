using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models.Keystore
{
    public class AddKeystoreRequest
    {
        [Required]
        public int GameId { get; set; }
        
        [Required]
        public List<string> Keys { get; set; }
    }
}