using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApiBase.Models;
using WebApiBase.Services;
using WebApiBase.Services.UserManager;

namespace WebApiBase.Controllers
{
    [Route("UserInfo")]
    [ApiController]
    public class UserInfoController : Controller
    {
        private readonly IUser _iuser;
        public UserInfoController(IUser _iuser)
        {
            this._iuser = _iuser;
        }

        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns>测试返回格式</returns>
        [HttpPost]
        [Authorize]
        [Route("Test")]
        public async Task<ResponseVo<object>> Test()
        {
            var test =await _iuser.Test();
            return test;
        }
        
        [HttpPut]
        [Route("Register")]
        public async Task<IActionResult> Register(UserInfo userInfo)
        {
            var res =await _iuser.Register(userInfo);
            return Json(res);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(InputUserModel userInfo)
        {
            var res = await _iuser.Login(userInfo);
            return Json(res);
        }

        [HttpGet]
        [Route("singleuserinfo")]
        public async Task<IActionResult> SingleUserInfo(string id)
        {
            var res = await _iuser.SingleUserInfo(id);
            return Json(res);
        }
    }
}
