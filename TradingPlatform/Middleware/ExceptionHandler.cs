using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TradingPlatform.Models;

namespace TradingPlatform.Middleware
{
    /// <summary>
    /// Middleware для централизованной обработки исключений
    /// </summary>
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;

        public ExceptionHandler(RequestDelegate next) =>
            this.next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception exception)
            {
                await ExceptionHandling(context, exception);
            }
        }

        private async Task ExceptionHandling(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int) HttpStatusCode.InternalServerError;

            await response.WriteAsync(
                JsonConvert.SerializeObject(
                    new
                    {
                        error = new Error {Message = exception.Message}
                    }
                )
            );
        }
    }
}