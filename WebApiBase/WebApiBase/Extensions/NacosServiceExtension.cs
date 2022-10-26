using Nacos.Microsoft.Extensions.Configuration;
using Nacos.V2;
using Nacos.V2.DependencyInjection;
using System.Text;

namespace WebApiBase.Extensions
{
    public static class NacosServiceExtension
    {
        public static WebApplicationBuilder AddNacosService(this WebApplicationBuilder builder)
        {
            builder.Services.AddNacosV2Config(x =>
            {
                //c.GetSection("NacosConfig");
                x.ServerAddresses = new List<string> { "http://120.53.244.123:8849/" };
                x.Namespace = "public";
                x.UserName = "nacos";
                x.Password = "nacos";
                //x.Listeners = new List<Nacos.Microsoft.Extensions.Configuration.ConfigListener>();
                //Nacos.Microsoft.Extensions.Configuration.ConfigListener model = new Nacos.Microsoft.Extensions.Configuration.ConfigListener();
                //model.DataId = "";
                //model.Group = "";
                //model.Optional = false;
                // swich to use http or rpc
                x.ConfigUseRpc = false;
            });

            IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
            var configSvc = serviceProvider.GetService<INacosConfigService>();
            string dataId = "nacosConfig";
            string group = "DEFAULT_GROUP";
            var content = configSvc.GetConfig("nacosConfig", "DEFAULT_GROUP", 3000);
            builder.Host.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var config = new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(content.Result))).Build();
                var c = configBuilder.AddConfiguration(config);
                var test = c.Build();
                var test2 = test.GetSection("test").Value;
            });

            Console.WriteLine(content);

            var listener = new ConfigListener(builder);

            //添加监听
            Task.Run(async () =>
            {
                await configSvc.AddListener(dataId, group, listener);
            });

            //发布配置
            //var isPublishOk = await configSvc.PublishConfig(dataId, group, "content");
            //Console.WriteLine(isPublishOk);
            //await Task.Delay(3000);

            //删除配置
            //var isRemoveOk = await configSvc.RemoveConfig(dataId, group);
            //Console.WriteLine(isRemoveOk);
            //await Task.Delay(3000);
            return builder;
        }
    }

    public class ConfigListener : IListener
    {
        private readonly WebApplicationBuilder builder;
        public ConfigListener(WebApplicationBuilder builder)
        {
            this.builder = builder;
        }
        public void ReceiveConfigInfo(string configInfo)
        {
            builder.Host.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var config = new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(configInfo))).Build();
                _ = configBuilder.AddConfiguration(config);
            });
        }
    }
}