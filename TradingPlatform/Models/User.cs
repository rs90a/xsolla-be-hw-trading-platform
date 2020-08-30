using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TradingPlatform.Models
{
    /// <summary>
    /// Пользователь системы
    /// </summary>
    public class User : IdentityUser
    {
        [Required]
        public List<string> Roles { get; set; }
        
        [Url]
        public string NotificationUrl { get; set; }
    }
}