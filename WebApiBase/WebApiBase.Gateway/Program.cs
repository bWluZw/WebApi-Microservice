using Nacos.AspNetCore.V2;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// �����־���� �������ڷ������ؽӿڵ�ʱ������ڿ���̨��ӡ�����Ӧ����Ϣ
builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});


//��nacos��ӵ����ö�ȡ�����滻��ԭ�е�IConfigurationBuilder����֧���ȸ���

//builder.Services.AddNacosAspNet(builder.Configuration, "NacosConfig");

//var test = builder.Configuration.GetSection("ConnectionStrings");

builder.Host.ConfigureAppConfiguration((context, configBuilder) =>
{
    var c = configBuilder.Build();
    
    var test= configBuilder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
    Console.WriteLine(c.GetSection("NacosConfig"));

});
// ע��Ocelot ����
builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.
// ----------
// ע���Ocelot ����� �������м��
app.UseOcelot().Wait();

app.UseAuthorization();

app.MapControllers();

app.Run();
