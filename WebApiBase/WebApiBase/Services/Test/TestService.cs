using System.Xml;
using WebApiBase.Models;

namespace WebApiBase.Services.Test
{
    public class TestService : BaseService, ITest
    {
        private readonly Nacos.V2.INacosNamingService _svc;
        private readonly IConfiguration _configuration;
        public TestService(IConfiguration _configuration, Nacos.V2.INacosNamingService svc)
        {
            this._configuration = _configuration;
            this._svc = svc;
        }

        public async Task<ResponseVo<object>> CallNacosService()
        {
            var res = await Task.Run(async () =>
           {

               // 这里需要知道被调用方的服务名
               var instance = await _svc.SelectOneHealthyInstance("BaseApi", "DEFAULT_GROUP");
               var host = $"{instance.Ip}:{instance.Port}";

               var baseUrl = instance.Metadata.TryGetValue("secure", out _)
                   ? $"https://{host}"
                   : $"http://{host}";

               if (string.IsNullOrWhiteSpace(baseUrl))
               {
                   return "empty";
               }

               var url = $"{baseUrl}/api/values";

               using (var client = new HttpClient())
               {
                   var result = await client.GetAsync(url);
                   return await result.Content.ReadAsStringAsync();
               };
           });
            return Success(res);
        }

        public async Task<ResponseVo<object>> NacosTest()
        {
            var test = _configuration.GetSection("test").Value;
            var test2 = _configuration.GetSection("log4net").GetChildren();
            var test1 = _configuration["log4net"];
            var test3 = _configuration.GetValue<XmlElement>("log4net");
            var test4 = _configuration.AsEnumerable().AsEnumerable();
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
            var res = await Task.Run(() =>
             {
                 return Success("11111111111");

             });
            return res;
        }
    }
}
