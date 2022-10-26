using Autofac;
using Autofac.Extensions.DependencyInjection;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nacos.AspNetCore.V2;
using Nacos.V2;
using Nacos.V2.DependencyInjection;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using WebApiBase.Controllers;
using WebApiBase.DatabaseAccessor;
using WebApiBase.Db;
using WebApiBase.Extensions;
using WebApiBase.Filters;
using WebApiBase.Models;
using WebApiBase.Utils;

var builder = WebApplication.CreateBuilder(args);
{ 
    
}

//�ر��Դ�logging
builder.Host.ConfigureLogging((hostingContext, loggingBuilder) =>
{
    loggingBuilder.ClearProviders();
});

//builder.Services.AddDbContext<BaseDbContext>(option=>option)
//builder.Services.AddNacosAspNet(builder.Configuration, "NacosConfig");

//var test = builder.Configuration.GetSection("test");


//builder.Services.AddNacosV2Config(x =>
//{
//    x.ServerAddresses = new System.Collections.Generic.List<string> { "http://120.53.244.123:8849/" };

//    x.EndPoint = "";
//    x.Namespace = "public";

//    x.UserName = "nacos";
//    x.Password = "nacos";

//    // swich to use http or rpc
//    x.ConfigUseRpc = true;
//});

//builder.Services.AddNacosV2Naming(x =>
//{
//    x.ServerAddresses = new System.Collections.Generic.List<string> { "http://120.53.244.123:8849/" };
//    x.EndPoint = "";
//    x.Namespace = "public";

//    x.UserName = "nacos";
//    x.Password = "nacos";

//    // swich to use http or rpc
//    x.NamingUseRpc = true;
//});


//builder.Services.AddNacosAspNet(builder.Configuration, "NacosConfig");



//Host.CreateDefaultBuilder(args)
//    .ConfigureWebHostDefaults((config) =>
//    {

//    })
//    .ConfigureAppConfiguration((context, builder) =>
//{
//    var c = builder.Build();
//    builder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
//});


//����������Զ�̬ʵ�����ã�����δ����
//log4net.Config.XmlConfigurator.Configure();
//ע��Log4Net
builder.Services.AddLogging(cfg =>
{
    cfg.Configure((opt) =>
    {
        opt.ActivityTrackingOptions = ActivityTrackingOptions.None;
    });
    //cfg.AddLog4Net();
    //log4net.Config.XmlConfigurator.Configure();
    //Ĭ�ϵ������ļ�·�����ڸ�Ŀ¼�����ļ���Ϊlog4net.config
    //����ļ�·���������б仯����Ҫ����������·��������
    //��������Ŀ��Ŀ¼�´���һ����Ϊcfg���ļ��У���log4net.config�ļ��������У�������Ϊlog.config
    //����Ҫʹ������Ĵ�������������
    cfg.AddLog4Net(new Log4NetProviderOptions()
    {
        Log4NetConfigFileName = "log4net.config",
        Watch = true
    });
});



//���jwt��֤��
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    var keyByte = Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]);
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        //��֤������
//        ValidateIssuer = true,
//        ValidIssuer = builder.Configuration["JWT:Issuer"],
//        //��֤������
//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["JWT:Audience"],
//        //��֤�Ƿ����
//        ValidateLifetime = true,
//        //��֤˽Կ
//        IssuerSigningKey = new SymmetricSecurityKey(keyByte),
//        ClockSkew = TimeSpan.Zero,
//    };
//});

#region core�Դ�������ע��

//////����Jwt�ĸ��ֲ���
//builder.Services.AddScoped<JwtSecurityTokenHandler>();
//////�Լ�д��֧�ַ��ʹ���Jwt ������չ
//builder.Services.AddScoped<TokenHelper>();
////����ע���Զ���log4
builder.Services.AddSingleton<WebApiBase.Utils.Log4.ILoggerHandler, WebApiBase.Utils.Log4.LoggerHandler>();

//builder.Services.AddScoped<HomeController>();
#endregion


#region �ⲿ���ݿ�����


DbProviderFactories.RegisterFactory("MySql", MySqlConnector.MySqlConnectorFactory.Instance);
DbProviderFactories.RegisterFactory("Oracle", Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);
DbProviderFactories.RegisterFactory("SqlServer", System.Data.SqlClient.SqlClientFactory.Instance);


#endregion

// Ҳ�����������ã�����ָ�����������ʣ�
// builder.WithOrigins("http://example.com",
//                  "http://www.contoso.com")
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials();

builder.Services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true)));


//builder.Services.AddControllers()
//    .AddNewtonsoftJson(i => i.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddControllers()
    .AddNewtonsoftJson(builder =>
    {
        //��ֵ����
        builder.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        //����ѭ������
        builder.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        //ȫ��С�շ�ת���շ�
        //builder.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        //builder.SerializerSettings.Formatting = Formatting.Indented;

        builder.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
        builder.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

        builder.ReadJsonWithRequestCulture = true;

    });


builder.Services.AddMvc(option =>
{
    //���Ȩ�޹�����,���ø����м��
    //option.Filters.Add<AuthorityFilter>();
    //�쳣�����������ø����м��
    //option.Filters.Add<ExceptionGlobalFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

#region �°�swagger

// ���swagger api docs
builder.Services.AddSwaggerGen(sw =>
{
    //sw.OperationFilter<AddAuthTokenHeaderParameter>();
    //sw.SwaggerDoc("v1", new OpenApiInfo { Title = "API V1", Version = "v1" });
    sw.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AspApi",
        Description = "Api",
        Contact = new OpenApiContact() { Name = "bWluZw" },
        Version = "v1"
    });

    var basePath = AppContext.BaseDirectory;
    var xmlPath = Path.Combine(basePath, "WebApiBase.xml");
    sw.IncludeXmlComments(xmlPath, true); //�ӿ�ע����Ϣ
    //sw.IncludeXmlComments(Path.Combine(basePath, "WebApiBase.xml"), true);

    //var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList();
    //foreach (var item in files)
    //{
    //    sw.IncludeXmlComments(item, true);
    //}
    sw.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    sw.CustomOperationIds(apiDesc =>
    {
        var controllerAction = apiDesc.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
        return controllerAction.ControllerName + "-" + controllerAction.ActionName;
    });
});

#endregion

builder.Services.AddEndpointsApiExplorer();
#region �ɰ�swagger



//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Version = "v1.0",
//        Title = "SwaggerShow",
//        Description = "�ӿ�˵���ĵ�",
//        Contact = new OpenApiContact { Name = "BiovBlue", Email = "BiovBlue.com" }

//    }); ; //END SwaggerDoc()

//    var basePath = AppContext.BaseDirectory;
//    var xmlPath = Path.Combine(basePath, "WebApiBase.xml");
//    options.IncludeXmlComments(xmlPath, true); //�ӿ�ע����Ϣ
//    options.IncludeXmlComments(Path.Combine(basePath, "WebApiBase.xml"), true); //Modelע����Ϣ


//    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        //������Ϣ
//        Description = "���������Bearer��Token������ ��Bearer {Token}�� ",
//        //Header��Ӧ����
//        Name = "Authorization",
//        //��֤���ͣ��˴�ʹ��Api Key
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
//        //���� API ��Կ��λ��
//        In = Microsoft.OpenApi.Models.ParameterLocation.Header
//    });
//});
#endregion


builder.AddNacosService();


//IConfiguration config = new ConfigurationBuilder().AddJsonFile("").Build();

//string test = config.GetSection("AppSetting")[""];

//builder.Services.AddNacosV2Config(x =>
//{
//    //c.GetSection("NacosConfig");
//    x.ServerAddresses = new List<string> { "http://120.53.244.123:8849/" };
//    x.Namespace = "public";
//    x.UserName = "nacos";
//    x.Password = "nacos";
//    //x.Listeners = new List<Nacos.Microsoft.Extensions.Configuration.ConfigListener>();
//    //Nacos.Microsoft.Extensions.Configuration.ConfigListener model = new Nacos.Microsoft.Extensions.Configuration.ConfigListener();
//    //model.DataId = "";
//    //model.Group = "";
//    //model.Optional = false;
//    // swich to use http or rpc
//    x.ConfigUseRpc = false;
//});


//builder.Host.ConfigureAppConfiguration((context, configBuilder) =>
//{
    
//    var c = configBuilder.Build();
//    var test = configBuilder.AddNacosV2Configuration((x) =>
//    {
//        //c.GetSection("NacosConfig");
//        x.ServerAddresses = new List<string> { "http://120.53.244.123:8849/" };
//        x.Namespace = "public";
//        x.UserName = "nacos";
//        x.Password = "nacos";
//        x.Listeners = new List<Nacos.Microsoft.Extensions.Configuration.ConfigListener>();
//        Nacos.Microsoft.Extensions.Configuration.ConfigListener model = new Nacos.Microsoft.Extensions.Configuration.ConfigListener();
//        model.DataId = "";
//        model.Group = "";
//        model.Optional = false;
//        // swich to use http or rpc
//        x.ConfigUseRpc = false;
//    });

//    //IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
//    //var configSvc = serviceProvider.GetService<INacosConfigService>();

//    //var test = configBuilder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
//    var test3 = test.Build();
    
//    var test2 = builder.Configuration;
//    //var c = configBuilder.Build();

//});


//���ݿ�����

//var connectionString = builder.Configuration.GetSection("DbInfo").GetSection("Db1")["DbType"];
var dbInfoList = builder.Configuration.GetSection("DbInfo").GetChildren();
foreach (var item in dbInfoList)
{
    var dbType = item["DbType"];
    var dbConn = item["ConnectionStr"];
}

//ʹ��DbContext���ӳأ���������ef�������Լ�����󲿷ֵ����ݿ����ӳص�����
builder.Services.AddDbContextPool<BaseDbContext>((opt) =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    opt.UseMySql(connectionString, serverVersion);


    //sql����������ʱ����
    //opt.AddInterceptors(new SqlCommandProfilerInterceptor());
    opt.LogTo((msg) =>
    {
        System.Diagnostics.Debug.WriteLine(Environment.NewLine + "+++++++++++++start+++++++++++++");
        System.Diagnostics.Debug.WriteLine(msg);
        System.Diagnostics.Debug.WriteLine("++++++++++++++end++++++++++++++" + Environment.NewLine);
    }, Microsoft.Extensions.Logging.LogLevel.Information);
    //CodeFirstǨ�����ݿⲽ�裺
    //Add-Migration  //����Ǩ�Ƽ�¼
    //EditMifrationName  //�����Զ����Ǩ�Ƽ�¼��
    //Remove-Migration  //ɾ���ո����ɵ�Ǩ�Ƽ�¼
    //Update-Database  //Ǩ�������ݿ�
}, poolSize: 64);


#region ��ֹĬ����Ϊ
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = (context) =>
    {
        if (context.ModelState.IsValid)
            return null;
        var error = "";
        foreach (var item in context.ModelState)
        {
            var state = item.Value;
            string message = "";
            //����ǿ�������������쳣��Ϣ
            if (builder.Environment.IsDevelopment())
            {
                message = state.Errors.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ToString()))?.ErrorMessage;
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = state.Errors.FirstOrDefault(o => o.Exception != null)?.Exception.ToString();
                }
            }
            else
            {
                message = state.Errors.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Exception.Message))?.ErrorMessage;
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = state.Errors.FirstOrDefault(o => o.Exception != null)?.Exception.Message;
                }
            }
            if (string.IsNullOrWhiteSpace(message))
                continue;
            error = message;
            break;
        }
        return new JsonResult(error);
    };
});
#endregion




//autofac����
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    //JWT��ع��ߺ�����
    builder.RegisterType<JwtSecurityTokenHandler>();
    builder.RegisterType<TokenHelper>();


    //�Զ�ע���dllȫ��
    Assembly assembly = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
    builder.RegisterAssemblyTypes(assembly)
           .AsImplementedInterfaces()
           .InstancePerDependency();

    ////ʵ�ֲִ�ģʽ
    //builder.RegisterAssemblyTypes(Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name))
    //    .Where(t => t.Name.EndsWith("Repository"))
    //    .AsImplementedInterfaces();

    //ע��ִ�������IRepository�ӿڵ�Repository��ӳ��
    builder.RegisterGeneric(typeof(Repository<>))
        //InstancePerDependency��Ĭ��ģʽ��ÿ�ε��ã���������ʵ��������ÿ�����󶼴���һ���µĶ���
        .As(typeof(IRepository<>))
        .InstancePerDependency();

    ////ע���ⲿ���ݿ�ִ�
    //builder.RegisterGeneric(typeof(ExtraRepository<>))
    //.As(typeof(IExtraRepository<>))
    //.InstancePerDependency();


});

var app = builder.Build();


// Configure the HTTP request pipeline.
#region �ɰ�swagger

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseExceptionHandleMiddleware();
#endregion


//ʹ��JWT�м��

app.UseCors("MyAllowSpecificOrigins");
app.UseJWTMiddlewareExtension();

app.UseRouting();
//JWT���
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
#region �°�swagger

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        var updatedPaths = new OpenApiPaths();
        foreach (var entry in swaggerDoc.Paths)
        {
            updatedPaths.Add(
                entry.Key.Replace("v{version}", swaggerDoc.Info.Version),
                entry.Value);
        }
        swaggerDoc.Paths = updatedPaths;
    });
});
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api V1");

//    c.InjectJavascript(Path.Combine(AppContext.BaseDirectory), "/Swagger/swagger.js");
//});
app.UseKnife4UI(c =>
{
    c.RoutePrefix = string.Empty; ; // serve the UI at root
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP API V1");
});
app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseHsts();
//}

#endregion
app.Run();
