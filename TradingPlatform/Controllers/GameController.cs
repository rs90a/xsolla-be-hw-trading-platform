using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Game;

namespace TradingPlatform.Controllers
{
    /// <summary>
    /// API для работы с товарами торговой площадки
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "seller")]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        /// <summary>
        /// Получение всех игр с загруженными игровыми ключами
        /// </summary>
        [AllowAnonymous, HttpGet("Games")]
        public async Task<IActionResult> GetGamesWithKeys()
        {
            return new OkObjectResult(new
            {
                games = await gameService.GetGamesWithKeys()
            });
        }
        
        /// <summary>
        /// Получение всех игр продавца
        /// </summary>
        [HttpGet("MyGames")]
        public async Task<IActionResult> GetMyGames()
        {
            return new OkObjectResult(new
            {
                games = await gameService.GetMyGames()
            });
        }

        /// <summary>
        /// Добавление новой игры
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddGame([FromBody] AddGameRequest addGameRequest)
        {
            return new OkObjectResult(new
            {
                message = "Игра успешно добавлена",
                game = await gameService.AddGame(addGameRequest)
            });
        }
    }
}