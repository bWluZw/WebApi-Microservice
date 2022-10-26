using WebApiBase.Models;

namespace WebApiBase.Services.Test
{
    public class TestService : ITest
    {
        private readonly Nacos.V2.INacosNamingService _svc;
        private readonly IConfiguration _configuration;
        public TestService(IConfiguration _configuration)
        {
            this ._configuration = _configuration;
        }
        public async Task<ResponseVo<string>> NacosTest()
        {
            var test = _configuration.GetSection("test").Value;
            var test1 = _configuration["test"];
            //// 这里需要知道被调用方的服务名
            //var instance = await _svc.SelectOneHealthyInstance("App2", "DEFAULT_GROUP");
            //var host = $"{instance.Ip}:{instance.Port}";

            //var baseUrl = instance.Metadata.TryGetValue("secure", out _)
            //    ? $"https://{host}"
            //    : $"http://{host}";


            //var url = $"{baseUrl}/api/values";

            //using (HttpClient client = new HttpClient())
            //{
            //    var result = await client.GetAsync(url);
            //    var res = await result.Content.ReadAsStringAsync();
            //}
            throw new Exception("");
        }
    }
}
