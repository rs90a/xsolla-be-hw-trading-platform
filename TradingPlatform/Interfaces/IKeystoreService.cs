using System.Threading.Tasks;
using TradingPlatform.Models.Keystore;

namespace TradingPlatform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с игровыми ключами
    /// </summary>
    public interface IKeystoreService
    {
        public Task AddKeystore(AddKeystoreRequest addKeystoreRequest);
        public Task DeleteKey(int keyId);
    }
}