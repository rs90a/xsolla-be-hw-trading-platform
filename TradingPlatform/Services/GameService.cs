using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Database;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Game;
using TradingPlatform.Models.Join;
using TradingPlatform.Models.Keystore;

namespace TradingPlatform.Services
{
    /// <summary>
    /// Cервис для работы c товарами торговой площадки
    /// </summary>
    public class GameService : IGameService
    {
        private readonly TradingPlatformDbContext dbContext;
        private readonly IAccountService accountService;
        private readonly IKeystoreService keystoreService;

        public GameService(TradingPlatformDbContext dbContext, IAccountService accountService,
            IKeystoreService keystoreService)
        {
            this.dbContext = dbContext;
            this.accountService = accountService;
            this.keystoreService = keystoreService;
        }

        /// <summary>
        /// Получение всех игр с загруженными игровыми ключами
        /// </summary>
        public async Task<GameDto[]> GetGamesWithKeys()
        {
            var gamesJoinKeys = await (
                from game in dbContext.Games
                join key in dbContext.Keys on game.Id equals key.GameId
                select new GameJoinKey
                {
                    Game = game,
                    Key = key
                }
            ).ToListAsync();
            
            var gamesWithKeys = gamesJoinKeys.GroupBy(gameJoinKey => gameJoinKey.Game)
                .Select(group => group.Key)
                .ToArray();
            
            return gamesWithKeys;
        }

        /// <summary>
        /// Получение всех игр продавца
        /// </summary>
        public async Task<GameDto[]> GetMyGames()
        {
            var userId = accountService.GetCurrentUserId();
            
            return await dbContext.Games.Where(game => game.SellerId == userId).ToArrayAsync();
        }

        /// <summary>
        /// Добавление новой игры
        /// </summary>
        public async Task<GameDto> AddGame(AddGameRequest addGameRequest)
        {
            var game = GameToGameDto(addGameRequest);
            await dbContext.Games.AddAsync(game);
            await dbContext.SaveChangesAsync();
            
            var keystore = new AddKeystoreRequest
            {
                GameId = game.Id,
                Keys = addGameRequest.Keys
            };

            await keystoreService.AddKeystore(keystore);
            
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