using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TradingPlatform.Interfaces;
using TradingPlatform.Models;

namespace TradingPlatform.Services
{
    public class JwtService : IAuth
    {
        private readonly Jwt jwt;

        public JwtService(Jwt jwt) =>
            this.jwt = jwt;

        /// <summary>
        /// Создание токена
        /// </summary>
        public string CreateToken(User user)
        {
            var signingKey = GetSecurityKey(jwt.Key);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim("userId", user.Id)
            };

            user.Roles.ForEach(role =>
                claims.Add(new Claim(ClaimTypes.Role, role))
            );
            
            var nowTime = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                nowTime,
                nowTime.Add(TimeSpan.FromMinutes(jwt.LifeTime)),
                credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Получение security key
        /// </summary>
        public SecurityKey GetSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}