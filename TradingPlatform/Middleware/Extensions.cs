using Microsoft.AspNetCore.Builder;

namespace TradingPlatform.Middleware
{
    public static class Extensions
    {
        /// <summary>
        /// Метод расширения для встраивания компонента ExceptionHandler
        /// </summary>
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandler>();
        }
    }
}