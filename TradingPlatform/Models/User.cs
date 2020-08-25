using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TradingPlatform.Models
{
    /// <summary>
    /// Пользователь системы
    /// </summary>
    public class User : IdentityUser
    {
        public List<string> Roles { get; set; }
    }
}