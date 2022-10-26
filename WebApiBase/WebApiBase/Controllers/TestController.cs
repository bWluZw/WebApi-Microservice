using Microsoft.AspNetCore.Mvc;
using WebApiBase.Models;
using WebApiBase.Services.Test;

namespace WebApiBase.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : Controller
    {
        private readonly ITest test;
        public TestController(ITest test)
        {
            this.test = test;
        }
        [Route("Test")]
        [HttpGet]
        public async Task<ResponseVo<string>> Index()
        {
            return await test.NacosTest();
        }
    }
}
