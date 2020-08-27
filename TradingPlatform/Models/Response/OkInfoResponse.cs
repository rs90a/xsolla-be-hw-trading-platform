using Microsoft.AspNetCore.Mvc;

namespace TradingPlatform.Models.Response
{
    public class OkInfoResponse : OkObjectResult
    {
        public OkInfoResponse(string message) : base(new {message})
        {
        }
    }
}