using Microsoft.Extensions.Logging;

namespace SimplifiedIMDBApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Request received: {Method} {Path}", context.Request.Method, ex);
                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/error/404");
                }
                else
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("An unexpected error occurred!");
                }
            }
        }
    }
}
