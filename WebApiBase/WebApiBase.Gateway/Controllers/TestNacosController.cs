using Microsoft.AspNetCore.Mvc;

namespace WebApiBase.Gateway.Controllers
{
    [ApiController]
    [Route("home")]
    public class TestNacosController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            return Json("11111111111");
        }
    }
}
