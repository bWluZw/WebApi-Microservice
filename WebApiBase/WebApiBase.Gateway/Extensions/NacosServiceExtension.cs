using Nacos.V2;
using Nacos.V2.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace WebApiBase.Gateway.Extensions
{
    public static class NacosServiceExtension
    {
        /// <summary>
        /// 获取远程配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="section">appsettings.json的配置项名</param>
        /// <returns></returns>
        public static async Task<WebApplicationBuilder> AddNacosService(this WebApplicationBuilder builder, string section)
        {

            var config = builder.Configuration.GetSection(section);
            string Namespace = config.GetSection("Namespace").Value;
            var ServerAddresses = config.GetSection("ServerAddresses").GetChildren();
            string UserName = config.GetSection("UserName").Value;
            string Password = config.GetSection("Password").Value;
            string ConfigUseRpc = config.GetSection("ConfigUseRpc").Value;
            string NamingUseRpc = config.GetSection("NamingUseRpc").Value;
            var listeners = config.GetSection("Listeners").GetChildren();

            Dictionary<string, string> listenersDic = new Dictionary<string, string>();
            foreach (var item in listeners)
            {
                listenersDic.Add(item.GetSection("DataId").Value, item.GetSection("Group").Value);
            }

            List<string> addressList = new List<string>();
            foreach (var item in ServerAddresses)
            {
                addressList.Add(item.Value);
            }

            builder.Services.AddNacosV2Config(x =>
            {
                x.ServerAddresses = new List<string>(addressList);
                x.Namespace = Namespace;
                x.UserName = UserName;
                x.Password = Password;
                x.ConfigUseRpc = bool.Parse(ConfigUseRpc);
                x.NamingUseRpc = bool.Parse(NamingUseRpc);
            });

            IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
            var configSvc = serviceProvider.GetService<INacosConfigService>();

            await Parallel.ForEachAsync(listenersDic, async (item, valueTask) =>
            {
                await Task.Run(async () =>
                {
                    var content = await configSvc.GetConfig(item.Key, item.Value, 3000);
                    builder.Host.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        IConfigurationRoot config;
                        if (IsJson(content))
                        {
                            config = new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(content))).Build();
                        }
                        else
                        {
                            Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(content));
                            config = new ConfigurationBuilder().AddXmlStream(stream).Build();
                        }
                        var c = configBuilder.AddConfiguration(config);
                        _ = c.Build();
                    });

                    //添加监听
                    Task.Run(async () =>
                    {
                        var listener = new ConfigListener(builder);
                        await configSvc.AddListener(item.Key, item.Value, listener);
                    });
                });
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

        private static bool IsJson(string str)
        {
            try
            {
                JsonConvert.DeserializeObject(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool isXml(string str)
        {
            try
            {
                JsonConvert.DeserializeXmlNode(str);
                return true;
            }
            catch
            {
                return false;
            }
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
