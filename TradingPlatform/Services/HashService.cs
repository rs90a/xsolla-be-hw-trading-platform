using System;
using System.Security.Cryptography;
using System.Text;
using TradingPlatform.Interfaces;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Cервис хэширования
    /// </summary>
    public class HashService : IHashService
    {
        private const string Salt = "D+hWa18ptk6K+Qd3HG7S6w==";

        public string CreateHash(params string[] values)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                return BitConverter.ToString(
                        sha1.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(values))))
                    .Replace("-", "");
            }
        }
        
        public string CreateHash(string value) => 
            CreateHash(new string[] {value, Salt});
    }
}