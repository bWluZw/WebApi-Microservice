using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using WebApiBase.Models;
using WebApiBase.Utils;
using WebApiBase.Utils.Log4;

namespace WebApiBase.Middlewares
{
    public class ExceptionHandleMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILoggerHandler _logger;

        public ExceptionHandleMiddleware(RequestDelegate _next, IHostEnvironment _hostEnvironment,ILoggerHandler _logger)
        {
            this._next = _next;
            this._hostEnvironment = _hostEnvironment;
            this._logger = _logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string newLine = Environment.NewLine;
                //日志记录
                _logger.Error($"{newLine}栈堆：{newLine}{ex.ToString()}, {newLine}产生了一个错误信息: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ApplicationException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };
            string responseContext = "";
            if (_hostEnvironment.IsDevelopment())
            {
                string newLine = Environment.NewLine;
                string exMsg = "====开发环境====" + newLine;
                exMsg += "产生了一个错误信息: " + newLine + exception.Message + newLine;
                exMsg += "栈堆：" + newLine + exception.ToString().Replace("\r\n", newLine);
                responseContext = exMsg;
            }
            else
            {
                if (context.Response.StatusCode == 404)
                {
                    responseContext = JsonHandler.Fail(null, "不存在的URL!");
                }
                else if (context.Response.StatusCode == 400)
                {
                    responseContext = JsonHandler.Fail(null, "错误请求!");
                }
                else
                {
                    responseContext = JsonHandler.Fail(null, "服务器错误!");
                }
            }
            await context.Response.WriteAsync(responseContext);
        }
    }
}
