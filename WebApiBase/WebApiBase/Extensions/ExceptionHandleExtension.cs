using WebApiBase.Middlewares;

namespace WebApiBase.Extensions
{
    public static class ExceptionHandleExtension
    {
        public static WebApplication UseExceptionHandleMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandleMiddleware>();
            return app;
        }
    }
}
