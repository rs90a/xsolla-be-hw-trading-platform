using System.ComponentModel.DataAnnotations;
using TradingPlatform.Models.Game;
using TradingPlatform.Models.Keystore;

namespace TradingPlatform.Models.Join
{
    public class GameJoinKey
    {
        [Required]
        public GameDto Game { get; set; }
        
        [Required]
        public KeyDto Key { get; set; }
    }
}