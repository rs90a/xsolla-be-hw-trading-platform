using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingPlatform.Interfaces;
using TradingPlatform.Models;

namespace TradingPlatform.Controllers
{
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

        [AllowAnonymous, HttpPost("[action]")]
        public IActionResult Token([FromBody] SignIn signIn)
        {
            return new OkObjectResult(new
            {
                token = accountService.GetToken(signIn).Result
            });
        }
    }
}