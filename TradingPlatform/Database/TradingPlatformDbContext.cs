using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Models;

namespace TradingPlatform.Database
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public sealed class TradingPlatformDbContext: IdentityDbContext<User>
    {
        public TradingPlatformDbContext(DbContextOptions<TradingPlatformDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}