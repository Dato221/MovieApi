namespace SimplifiedIMDBApi.Middleware
{
    public static class MiddleWareExt
    {
        public static IApplicationBuilder UseRequestLogMiddleware(this IApplicationBuilder builder)
        {
             builder.UseMiddleware<CustomExceptionMiddleware>();
             builder.UseMiddleware<ErrorHandlingMiddleware>();
             return builder;
        }
    }
}
