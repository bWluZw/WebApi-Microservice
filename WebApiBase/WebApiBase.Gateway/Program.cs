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
// ע��Ocelot ����
builder.Services.AddOcelot();

//��nacos��ӵ����ö�ȡ�����滻��ԭ�е�IConfigurationBuilder����֧���ȸ���
Host.CreateDefaultBuilder(args)
.ConfigureAppConfiguration((context, configBuilder) =>
{
    var c = configBuilder.Build();
    configBuilder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
// ----------
// ע���Ocelot ����� �������м��
app.UseOcelot().Wait();

app.UseAuthorization();

app.MapControllers();

app.Run();
