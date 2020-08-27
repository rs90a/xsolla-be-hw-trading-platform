using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Database;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Game;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Cервис для работы c товарами торговой площадки
    /// </summary>
    public class GameService: IGameService
    {
        private readonly TradingPlatformDbContext dbContext;
        private readonly IAccountService accountService;

        public GameService(TradingPlatformDbContext dbContext, IAccountService accountService)
        {
            this.dbContext = dbContext;
            this.accountService = accountService;
        }

        /// <summary>
        /// Получение всех игр
        /// </summary>
        public async Task<GameDto[]> GetAllGames()
        {
            return await dbContext.Games.ToArrayAsync();
        }

        /// <summary>
        /// Добавление новой игры
        /// </summary>
        public async Task<GameDto> AddGame(AddGameRequest addGameRequest)
        {
            var game = GameToGameDto(addGameRequest);
            await dbContext.Games.AddAsync(game);
            await dbContext.SaveChangesAsync();
            return game;
        }

        private GameDto GameToGameDto(AddGameRequest game)
        {
            return new GameDto
            {
                Id = 0,
                Name = game.Name,
                Description = game.Description,
                Price = game.Price,
                SellerId = accountService.GetCurrentUserId()
            };
        }
    }
}