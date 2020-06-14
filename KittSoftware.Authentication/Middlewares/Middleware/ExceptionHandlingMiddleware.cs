using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Navyblue.Authentication.Middlewares.Middleware
{
    public class ExceptionHandlingMiddleware:INavyBlueMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            IExceptionHandlerFeature ex = context.Features.Get<IExceptionHandlerFeature>();
            if (ex != null)
            {
                //记录日志
                this.logger.LogError(exception: ex.Error, ex.Error.Message,"NavyBlue.ExceptionHandlingExtensions");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(ex.Error?.Message ?? "an error occure");
            }

            await this.next.Invoke(context);
        }
    }
}
