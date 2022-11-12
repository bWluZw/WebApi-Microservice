using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Nacos;
using Ocelot.Provider.Nacos.NacosClient.V2;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true)));

builder.Configuration.AddJsonFile("appsettings.json", false, true);
//var test2 = builder.Configuration.AddJsonFile("ocelotconfig.json", false, true);

builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddNacosAspNet(builder.Configuration, "nacos");

builder.Host.ConfigureAppConfiguration((context, builder) =>
{
    var config = builder.Build();
    builder.AddNacosV2Configuration(config.GetSection("nacos"));
});

builder.Services.AddOcelot(builder.Configuration).AddNacosDiscovery();
var app = builder.Build();


app.UseHttpLogging();
//øÁ”Ú
app.UseCors("cors");
app.UseOcelot().Wait();

app.Run();
