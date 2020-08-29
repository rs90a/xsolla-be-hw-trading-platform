using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Models;
using TradingPlatform.Models.Game;
using TradingPlatform.Models.Keystore;
using TradingPlatform.Models.Order;

namespace TradingPlatform.Database
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public sealed class TradingPlatformDbContext: IdentityDbContext<User>
    {
        public DbSet<GameDto> Games { get; set; }
        
        public DbSet<KeyDto> Keys { get; set; }
        
        public DbSet<OrderDto> Orders { get; set; }
        
        public DbSet<OrderGamesDto> OrderGames { get; set; }
        
        public DbSet<OrderSellersDto> OrderSellers { get; set; }
        
        public TradingPlatformDbContext(DbContextOptions<TradingPlatformDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<KeyDto>()
                .HasKey(entity => new { entity.Id, entity.Key, entity.GameId });
        }
    }
}