using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using WebApiBase.Models;
using WebApiBase.Utils;
using WebApiBase.Utils.Log4;

namespace WebApiBase.Filters
{
    public class ExceptionGlobalFilter : IExceptionFilter
    {

        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILoggerHandler logger;
        
        public ExceptionGlobalFilter(IHostEnvironment _hostEnvironment,ILoggerHandler logger)
        {
            this._hostEnvironment = _hostEnvironment;
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            string newLine = Environment.NewLine;
            string msg = "";
            int code = 0;
            logger.Error($"{newLine}====开发环境===={newLine}栈堆：{newLine}{ex.StackTrace}, {newLine}产生了一个错误信息: {ex.Message}");
            if (_hostEnvironment.IsDevelopment())
            {
                msg = $"{newLine}====开发环境===={newLine}栈堆：{newLine}{ex.StackTrace}, {newLine}产生了一个错误信息: {ex.Message}";
            }

            //应用程序业务级异常
            if (ex is ApplicationException)
            {
                code = 400;
            }
            else
            {
                // exception 系统级别异常
                code = 500;
            }

            ContentResult result = new ContentResult
            {
                StatusCode = code,
                ContentType = "application/json;charset=utf-8;",
                Content = msg
            };

            //if (_hostEnvironment.IsDevelopment())
            //ResponseVo<string> vo = new ResponseVo<string>();
            //vo.success = false;
            //vo.code = 400;
            //vo.msg = "系统错误！";
            //vo.data = null;
            //result.Content = JsonConvert.SerializeObject(vo);

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
