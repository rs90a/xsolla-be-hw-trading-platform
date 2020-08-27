using System.Threading.Tasks;
using TradingPlatform.Models;

namespace TradingPlatform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы c пользователями торговой площадки
    /// </summary>
    public interface IAccountService
    {
        public Task<string> GetToken(SignIn signIn);
        public string GetCurrentUserId();
    }
}