using Microsoft.AspNetCore.Mvc;

namespace WebApiBase.Gateway.Controllers
{
    public class TestNacosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
