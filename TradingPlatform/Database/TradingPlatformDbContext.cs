using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Models;
using TradingPlatform.Models.Game;

namespace TradingPlatform.Database
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public sealed class TradingPlatformDbContext: IdentityDbContext<User>
    {
        public DbSet<GameDto> Games { get; set; }
        
        public TradingPlatformDbContext(DbContextOptions<TradingPlatformDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}