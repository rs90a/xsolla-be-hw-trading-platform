using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingPlatform.Models
{
    public class UserConfig : SignIn
    {
        [Required]
        public List<string> Roles { get; set; }
    }
}