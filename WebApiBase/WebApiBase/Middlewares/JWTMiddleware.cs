using System.IdentityModel.Tokens.Jwt;
using WebApiBase.Models;
using WebApiBase.Utils;

namespace WebApiBase.Middlewares
{
    public class JWTMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly TokenHelper _tokenHelper;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JWTMiddleware(RequestDelegate next, TokenHelper _tokenHelper, JwtSecurityTokenHandler _jwtSecurityTokenHandler)
        {
            _next = next;
            this._tokenHelper = _tokenHelper;
            this._jwtSecurityTokenHandler = _jwtSecurityTokenHandler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //此处写中间件业务逻辑
            //然后调用next指向下一个中间件
            System.Diagnostics.Debug.WriteLine("经过JWTMiddleware中间件");

            string oriToken = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(oriToken))
            {

            }
            else
            {
                string token = oriToken.Replace("Bearer ", "");
                TokenModel tokenModel = _tokenHelper.ReadToken(token);
                if (tokenModel == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(JsonHandler.Fail(null, "没有权限!", 401));
                    return;
                }
                else
                {
                    long.TryParse(tokenModel.exp, out long resExp);
                    //如果失效时间还有不到十分钟，那就刷星token
                    if (DateTime.UtcNow.Ticks - resExp <= 60 * 10 && DateTime.UtcNow.Ticks - resExp > 0)
                    {
                        context.Response.Headers["Authorization"] = _tokenHelper.CreateJwtToken<UserInfo>(tokenModel.UserInfo);
                    }
                }
            }

            //context.

            //调用next之前，属于请求部分的处理
            await _next(context);
            //next执行完毕之后，属于相应部分的处理
            if (context.Response.StatusCode == 401)
            {
                await context.Response.WriteAsync(JsonHandler.Fail(null, "没有权限!", 401));
            }
        }
    }
}
