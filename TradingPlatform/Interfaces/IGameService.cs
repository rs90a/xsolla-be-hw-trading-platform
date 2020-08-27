using System.Threading.Tasks;
using TradingPlatform.Models.Game;

namespace TradingPlatform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с товарами торговой площадки
    /// </summary>
    public interface IGameService
    {
        public Task<GameDto[]> GetGamesWithKeys();
        public Task<GameDto[]> GetMyGames();
        public Task<GameDto> AddGame(AddGameRequest addGameRequest);
    }
}