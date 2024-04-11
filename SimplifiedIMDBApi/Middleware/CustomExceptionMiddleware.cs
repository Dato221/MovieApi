using Newtonsoft.Json;
using System.Net;

namespace SimplifiedIMDBApi.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, ILogger<CustomExceptionMiddleware> logger)
        {
            logger.LogDebug("Request received: {Method} {Path}", context.Request.Method, context.Request.Path);
            await _next(context);
            logger.LogDebug("Response sent: {StatusCode}", context.Response.StatusCode);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var responce = new
            {
                status = code,
                detail = exception.Message,
                Message = "something went wrong"
            };
            var result = JsonConvert.SerializeObject(responce);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}

