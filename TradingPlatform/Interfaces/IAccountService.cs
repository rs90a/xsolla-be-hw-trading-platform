using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        public Task<IdentityResult> AddUser(SignUp signUp);
    }
}