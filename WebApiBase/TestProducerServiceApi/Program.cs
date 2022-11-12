using Nacos.AspNetCore.V2;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddControllers();
var test = builder.Configuration.AddJsonFile("appsettings.json", false, true);
//await builder.AddNacosService("nacos");

builder.Services.AddNacosAspNet(builder.Configuration);

builder.Host.ConfigureAppConfiguration((context, builder) =>
{
    var config = builder.Build();
    builder.AddNacosV2Configuration(config.GetSection("nacos"));
});

//IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
builder.Services.AddSingleton<IServiceProvider>(builder.Services.BuildServiceProvider());

var app = builder.Build();

app.MapControllers();

app.Run();
