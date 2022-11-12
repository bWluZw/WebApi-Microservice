using Microsoft.AspNetCore.Mvc;

namespace TestProducerServiceApi.Controllers
{
    [ApiController]
    [Route("producer")]
    public class TestController : Controller
    {
        [Route("test")]
        public IActionResult Index()
        {
            return Json("producer success!");
        }
    }
}
