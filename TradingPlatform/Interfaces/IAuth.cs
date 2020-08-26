using Microsoft.IdentityModel.Tokens;
using TradingPlatform.Models;

namespace TradingPlatform.Interfaces
{
    public interface IAuth
    {
        public string CreateToken(User user);
        public SecurityKey GetSecurityKey(string key);
    }
}