﻿using System.Threading.Tasks;
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
        /// Получение всех игр
        /// </summary>
        [AllowAnonymous, HttpGet("Games")]
        public async Task<IActionResult> GetAllGames()
        {
            return new OkObjectResult(new
            {
                games = await gameService.GetAllGames()
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
                game = await gameService.AddGame(addGameRequest)
            });
        }
    }
}