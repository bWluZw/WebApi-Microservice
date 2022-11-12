using Microsoft.AspNetCore.Mvc;
using Nacos.V2;

namespace TestConsumerServiceApi.Controllers
{
    [ApiController]
    [Route("consumer")]
    public class TestController : Controller
    {
        private IServiceProvider serviceProvider;
        private Nacos.V2.INacosNamingService svc;
        public TestController(IServiceProvider serviceProvider, Nacos.V2.INacosNamingService svc)
        {
            this.serviceProvider = serviceProvider;
            this.svc = svc;
        }
        //[Route("test")]
        //public async Task<IActionResult> Index()
        //{
        //    var namingSvc = serviceProvider.GetService<INacosNamingService>();

        //    // await namingSvc.RegisterInstance("BaseApi", "11.11.11.11", 8888, "TEST1");

        //    //await namingSvc.RegisterInstance("BaseApi", "2.2.2.2", 9999, "DEFAULT");
        //    var test = await namingSvc.GetAllInstances("BaseApi");
        //    var test2 = Newtonsoft.Json.JsonConvert.SerializeObject(test);

        //    //IServiceCollection services = new ServiceCollection();
        //    //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
        //    //    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        //    //services.AddNacosAspNet(configurationBuilder.Build());

        //    //var provider = services.BuildServiceProvider();
        //    //RegSvcBgTask regSvcBgTask = serviceProvider.GetRequiredService<RegSvcBgTask>();
        //    //await regSvcBgTask.StartAsync(new CancellationToken());
        //    return View();
        //}
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            var res = await Task.Run(async () =>
            {
                // 这里需要知道被调用方的服务名
                //var instance = await svc.SelectOneHealthyInstance("BaseApi", "DEFAULT_GROUP");
                var test = await svc.GetAllInstances("producerService1");
                var instance = test.FirstOrDefault(); 
                var host = $"{instance.Ip}:{instance.Port}";

                var baseUrl = instance.Metadata.TryGetValue("secure", out _)
                    ? $"https://{host}"
                    : $"http://{host}";

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    return "empty";
                }

                var url = $"{baseUrl}/producer/test";

                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(url);
                    return await result.Content.ReadAsStringAsync();
                };
            });
            return Json(res);
        }
    }
}
