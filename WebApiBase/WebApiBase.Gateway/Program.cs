using Nacos.AspNetCore.V2;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Nacos;
using WebApiBase.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 添加日志服务 ，这样在访问网关接口的时候可以在控制台打印输出相应的信息
builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});


////nacos添加的配置读取出来替换掉原有的IConfigurationBuilder，并支持热更新
await builder.AddNacosService("nacos");
// 注册Ocelot 服务
//这里有一个坑，AddNacosDiscovery默认nacos的配置名为nacos，并且内部做了一些处理，如果需要使用必须改为nacos
builder.Services.AddOcelot().AddNacosDiscovery();
//下面注释的不行，会提示没有注册包Ocelot.Provider.Nacos的某一个的服务
//builder.Services.AddOcelot(builder.Configuration);
//builder.Services.AddNacosAspNet(builder.Configuration, "NacosConfig");
//builder.Services.AddSingleton(NacosProviderFactory.Get);
//builder.Services.AddSingleton(NacosMiddlewareConfigurationProvider.Get);

var app = builder.Build();

// Configure the HTTP request pipeline.
// ----------
// 注册好Ocelot 服务后 启用其中间件
await app.UseOcelot();

app.UseAuthorization();

app.MapControllers();

app.Run();
