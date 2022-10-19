namespace WebApiBase.Extensions
{
    public static class JWTMiddlewareExtension
    {
        public static IApplicationBuilder UseJWTMiddlewareExtension(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Middlewares.JWTMiddleware>();
        }
    }
}
