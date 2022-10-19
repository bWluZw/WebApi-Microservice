using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using WebApiBase.Models;
using WebApiBase.Services.UserManager;

namespace WebApiBase.Utils
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public TokenHelper(IConfiguration configuration, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _configuration = configuration;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }


        public TokenModel ReadToken(string token)
        {
            //User userInfo = _tokenHelper.GetToken<claim>(token);
            if (_jwtSecurityTokenHandler.CanReadToken(token))
            {
                var test = _jwtSecurityTokenHandler.ReadToken(token);

            }
            var test1 = _jwtSecurityTokenHandler.ReadJwtToken(token);

            TokenModel model = new TokenModel();
            UserInfo userInfo = new UserInfo();
            userInfo.ID = Convert.ToInt32(test1.Claims.Where(i => i.Type == "ID").Select(i => i.Value).FirstOrDefault());
            userInfo.UserName = test1.Claims.Where(i => i.Type == "UserName").Select(i => i.Value).FirstOrDefault();
            userInfo.Pwd = test1.Claims.Where(i => i.Type == "Pwd").Select(i => i.Value).FirstOrDefault();
            userInfo.Role = test1.Claims.Where(i => i.Type == "Role").Select(i => i.Value).FirstOrDefault();
            model.nbf = test1.Claims.Where(i => i.Type == "nbf").Select(i => i.Value).FirstOrDefault();
            model.exp = test1.Claims.Where(i => i.Type == "exp").Select(i => i.Value).FirstOrDefault();
            model.iss = test1.Claims.Where(i => i.Type == "iss").Select(i => i.Value).FirstOrDefault();
            model.aud = test1.Claims.Where(i => i.Type == "aud").Select(i => i.Value).FirstOrDefault();

            model.UserInfo = userInfo;

            if (model.iss == _configuration["JWT:Issuer"] && model.aud == _configuration["JWT:Audience"])
            {
                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 创建加密JwtToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateJwtToken<T>(T user)
        {
            var claimList = this.CreateClaimList(user);
            //  从 appsettings.json 中读取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            // 从 appsettings.json 中读取Expires
            var expires = Convert.ToDouble(_configuration["JWT:Expires"]);
            //  选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;
            // 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
              _configuration["JWT:Issuer"],     //Issuer
               _configuration["JWT:Audience"],   //Audience
               claims: claimList,
               DateTime.Now,                    //notBefore
               DateTime.Now.AddHours(expires),   //expires
               signingCredentials               //数字签名
               );
            string jwtToken = _jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            return jwtToken;
        }

        public T GetToken<T>(string Token)
        {
            Type t = typeof(T);

            object objA = Activator.CreateInstance(t);
            var b = _jwtSecurityTokenHandler.ReadJwtToken(Token);
            foreach (var item in b.Claims)
            {
                PropertyInfo _Property = t.GetProperty(item.Type);
                if (_Property != null && _Property.CanRead)
                {
                    _Property.SetValue(objA, item.Value, null);
                }

            }
            return (T)objA;
        }


        /// <summary>
        /// 创建包含用户信息的CalimList，暂时没写角色
        /// </summary>
        /// <param name="authUser"></param>
        /// <returns></returns>
        private List<Claim> CreateClaimList<T>(T authUser)
        {
            var Class = typeof(T);
            List<Claim> claimList = new List<Claim>();
            foreach (var item in Class.GetProperties())
            {
                claimList.Add(new Claim(item.Name, Convert.ToString(item.GetValue(authUser))));
            }
            return claimList;
        }
    }
}
