using Nacos.AspNetCore.V2;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 添加日志服务 ，这样在访问网关接口的时候可以在控制台打印输出相应的信息
builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});


//讲nacos添加的配置读取出来替换掉原有的IConfigurationBuilder，并支持热更新

//builder.Services.AddNacosAspNet(builder.Configuration, "NacosConfig");

//var test = builder.Configuration.GetSection("ConnectionStrings");

builder.Host.ConfigureAppConfiguration((context, configBuilder) =>
{
    var c = configBuilder.Build();
    
    var test= configBuilder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
    Console.WriteLine(c.GetSection("NacosConfig"));

});
// 注册Ocelot 服务
builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.
// ----------
// 注册好Ocelot 服务后 启用其中间件
app.UseOcelot().Wait();

app.UseAuthorization();

app.MapControllers();

app.Run();
