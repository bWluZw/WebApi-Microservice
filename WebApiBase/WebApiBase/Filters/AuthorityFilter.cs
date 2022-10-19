using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using WebApiBase.Db;
using WebApiBase.Utils;

namespace WebApiBase.Filters
{
    public class AuthorityFilter : IAuthorizationFilter
    {
        private readonly BaseDbContext _context;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly TokenHelper _tokenHelper;
        public AuthorityFilter(BaseDbContext _context, JwtSecurityTokenHandler _jwtSecurityTokenHandler, TokenHelper _tokenHelper)
        {
            this._context = _context;
            this._jwtSecurityTokenHandler = _jwtSecurityTokenHandler;
            this._tokenHelper = _tokenHelper;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var heads = context.HttpContext.Request.Headers["Authorization"];

            //var a = _tokenHelper.GetToken<UserModel>(heads);

            //_serviceFactory.Get();
            //throw new Exception("");

            var test = context.ActionDescriptor;
            System.Diagnostics.Debug.WriteLine("授权过滤器已启动===============" + test.DisplayName);
        }
    }
}
