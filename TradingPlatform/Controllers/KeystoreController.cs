using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Keystore;
using TradingPlatform.Models.Response;

namespace TradingPlatform.Controllers
{
    /// <summary>
    /// API для работы с игровыми ключами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "seller")]
    public class KeystoreController
    {
        private readonly IKeystoreService keystoreService;

        public KeystoreController(IKeystoreService keystoreService)
        {
            this.keystoreService = keystoreService;
        }

        /// <summary>
        /// Добавление набора игровых ключей для игры
        /// </summary>
        [HttpPost("Add")]
        public async Task<IActionResult> AddKeystore(AddKeystoreRequest addKeystoreRequest)
        {
            await keystoreService.AddKeystore(addKeystoreRequest);
            return new OkInfoResponse("Игровые ключи успешно добавлены");
        }

        /// <summary>
        /// Удаление игрового ключа по id
        /// </summary>
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteKey(int id)
        {
            await keystoreService.DeleteKey(id);
            return new OkInfoResponse("Игровой ключ успешно удален");
        }
    }
}