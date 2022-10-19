using Newtonsoft.Json.Linq;
using WebApiBase.Models;

namespace WebApiBase.Services.UserManager
{
    public interface IUser
    {
        public Task<ResponseVo<object>> Test();
        public Task<ResponseVo<object>> Register(UserInfo userInfo);
        public Task<ResponseVo<object>> Login(InputUserModel userInfo);

        /// <summary>
        /// 获取单个用户所有信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseVo<object>> SingleUserInfo(string id);
    }
}
