using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingPlatform.Interfaces;
using TradingPlatform.Models;

namespace TradingPlatform.Controllers
{
    /// <summary>
    /// API для работы с пользователями торговой площадки
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Получение токена
        /// </summary>
        [AllowAnonymous, HttpPost("Token")]
        public IActionResult CreateToken([FromBody] SignIn signIn)
        {
            return new OkObjectResult(new
            {
                token = accountService.GetToken(signIn).Result
            });
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        [AllowAnonymous, HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] SignUp signUp)
        {
            var identityResult = await accountService.AddUser(signUp);
            
            if (identityResult.Succeeded)
                return new OkObjectResult(new
                {
                    success = identityResult.Succeeded,
                    message = "Регистрация успешна"
                });
            
            return new OkObjectResult(new
            {
                success = identityResult.Succeeded,
                message = "Не удалось выполнить регистрацию",
                errors = identityResult.Errors.Select(error => error.Description)
            });
        }
    }
}