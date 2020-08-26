using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models
{
    public class SignIn
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}