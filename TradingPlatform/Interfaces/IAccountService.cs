using System.Threading.Tasks;
using TradingPlatform.Models;

namespace TradingPlatform.Interfaces
{
    public interface IAccountService
    {
        public Task<string> GetToken(SignIn signIn);
    }
}