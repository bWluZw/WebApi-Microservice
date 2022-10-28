using Nacos.AspNetCore.V2;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Nacos;
using WebApiBase.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// �����־���� �������ڷ������ؽӿڵ�ʱ������ڿ���̨��ӡ�����Ӧ����Ϣ
builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});


////nacos��ӵ����ö�ȡ�����滻��ԭ�е�IConfigurationBuilder����֧���ȸ���
await builder.AddNacosService("nacos");
// ע��Ocelot ����
//������һ���ӣ�AddNacosDiscoveryĬ��nacos��������Ϊnacos�������ڲ�����һЩ���������Ҫʹ�ñ����Ϊnacos
builder.Services.AddOcelot().AddNacosDiscovery();
//����ע�͵Ĳ��У�����ʾû��ע���Ocelot.Provider.Nacos��ĳһ���ķ���
//builder.Services.AddOcelot(builder.Configuration);
//builder.Services.AddNacosAspNet(builder.Configuration, "NacosConfig");
//builder.Services.AddSingleton(NacosProviderFactory.Get);
//builder.Services.AddSingleton(NacosMiddlewareConfigurationProvider.Get);

var app = builder.Build();

// Configure the HTTP request pipeline.
// ----------
// ע���Ocelot ����� �������м��
await app.UseOcelot();

app.UseAuthorization();

app.MapControllers();

app.Run();
