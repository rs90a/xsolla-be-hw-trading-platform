using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingPlatform.Interfaces;
using TradingPlatform.Models.Payment;

namespace TradingPlatform.Controllers
{
    [ApiController]
    [Authorize(Roles = "seller,buyer")]
    [Route("api/[controller]")]
    public class PaymentController
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        /// <summary>
        /// API-метод для создания новой сессии
        /// </summary>
        /// <param name="paymentInfo">Сведения о платеже</param>
        /// <returns>Id сессии</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Session(PaymentInfo paymentInfo)
        {
            return new OkObjectResult(new
            {
                sessionId = await paymentService.CreateSession(paymentInfo)
            });
        }
    }
}