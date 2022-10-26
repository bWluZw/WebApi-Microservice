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

//关闭自带logging
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


//这个方法可以动态实现配置，但是未测试
//log4net.Config.XmlConfigurator.Configure();
//注入Log4Net
builder.Services.AddLogging(cfg =>
{
    cfg.Configure((opt) =>
    {
        opt.ActivityTrackingOptions = ActivityTrackingOptions.None;
    });
    //cfg.AddLog4Net();
    //log4net.Config.XmlConfigurator.Configure();
    //默认的配置文件路径是在根目录，且文件名为log4net.config
    //如果文件路径或名称有变化，需要重新设置其路径或名称
    //比如在项目根目录下创建一个名为cfg的文件夹，将log4net.config文件移入其中，并改名为log.config
    //则需要使用下面的代码来进行配置
    cfg.AddLog4Net(new Log4NetProviderOptions()
    {
        Log4NetConfigFileName = "log4net.config",
        Watch = true
    });
});



//添加jwt验证：
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    var keyByte = Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]);
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        //验证发布者
//        ValidateIssuer = true,
//        ValidIssuer = builder.Configuration["JWT:Issuer"],
//        //验证接收者
//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["JWT:Audience"],
//        //验证是否过期
//        ValidateLifetime = true,
//        //验证私钥
//        IssuerSigningKey = new SymmetricSecurityKey(keyByte),
//        ClockSkew = TimeSpan.Zero,
//    };
//});

#region core自带的依赖注入

//////用于Jwt的各种操作
//builder.Services.AddScoped<JwtSecurityTokenHandler>();
//////自己写的支持泛型存入Jwt 便于扩展
//builder.Services.AddScoped<TokenHelper>();
////依赖注入自定义log4
builder.Services.AddSingleton<WebApiBase.Utils.Log4.ILoggerHandler, WebApiBase.Utils.Log4.LoggerHandler>();

//builder.Services.AddScoped<HomeController>();
#endregion


#region 外部数据库配置


DbProviderFactories.RegisterFactory("MySql", MySqlConnector.MySqlConnectorFactory.Instance);
DbProviderFactories.RegisterFactory("Oracle", Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);
DbProviderFactories.RegisterFactory("SqlServer", System.Data.SqlClient.SqlClientFactory.Instance);


#endregion

// 也可以这样设置：允许指定的域名访问：
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
        //空值忽略
        builder.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        //忽略循环引用
        builder.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        //全局小驼峰转大驼峰
        //builder.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        //builder.SerializerSettings.Formatting = Formatting.Indented;

        builder.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
        builder.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

        builder.ReadJsonWithRequestCulture = true;

    });


builder.Services.AddMvc(option =>
{
    //添加权限过滤器,弃用改用中间件
    //option.Filters.Add<AuthorityFilter>();
    //异常过滤器，弃用改用中间件
    //option.Filters.Add<ExceptionGlobalFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

#region 新版swagger

// 添加swagger api docs
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
    sw.IncludeXmlComments(xmlPath, true); //接口注释信息
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
#region 旧版swagger



//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Version = "v1.0",
//        Title = "SwaggerShow",
//        Description = "接口说明文档",
//        Contact = new OpenApiContact { Name = "BiovBlue", Email = "BiovBlue.com" }

//    }); ; //END SwaggerDoc()

//    var basePath = AppContext.BaseDirectory;
//    var xmlPath = Path.Combine(basePath, "WebApiBase.xml");
//    options.IncludeXmlComments(xmlPath, true); //接口注释信息
//    options.IncludeXmlComments(Path.Combine(basePath, "WebApiBase.xml"), true); //Model注释信息


//    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        //描述信息
//        Description = "请输入带有Bearer的Token，形如 “Bearer {Token}” ",
//        //Header对应名称
//        Name = "Authorization",
//        //验证类型，此处使用Api Key
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
//        //设置 API 密钥的位置
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


//数据库配置

//var connectionString = builder.Configuration.GetSection("DbInfo").GetSection("Db1")["DbType"];
var dbInfoList = builder.Configuration.GetSection("DbInfo").GetChildren();
foreach (var item in dbInfoList)
{
    var dbType = item["DbType"];
    var dbConn = item["ConnectionStr"];
}

//使用DbContext连接池，可以提升ef的性能以及解决大部分的数据库连接池的问题
builder.Services.AddDbContextPool<BaseDbContext>((opt) =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    opt.UseMySql(connectionString, serverVersion);


    //sql拦截器，暂时弃用
    //opt.AddInterceptors(new SqlCommandProfilerInterceptor());
    opt.LogTo((msg) =>
    {
        System.Diagnostics.Debug.WriteLine(Environment.NewLine + "+++++++++++++start+++++++++++++");
        System.Diagnostics.Debug.WriteLine(msg);
        System.Diagnostics.Debug.WriteLine("++++++++++++++end++++++++++++++" + Environment.NewLine);
    }, Microsoft.Extensions.Logging.LogLevel.Information);
    //CodeFirst迁移数据库步骤：
    //Add-Migration  //生成迁移记录
    //EditMifrationName  //输入自定义的迁移记录名
    //Remove-Migration  //删除刚刚生成的迁移记录
    //Update-Database  //迁移至数据库
}, poolSize: 64);


#region 禁止默认行为
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
            //如果是开发环境就输出异常信息
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




//autofac配置
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    //JWT相关工具和配置
    builder.RegisterType<JwtSecurityTokenHandler>();
    builder.RegisterType<TokenHelper>();


    //自动注册该dll全局
    Assembly assembly = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
    builder.RegisterAssemblyTypes(assembly)
           .AsImplementedInterfaces()
           .InstancePerDependency();

    ////实现仓储模式
    //builder.RegisterAssemblyTypes(Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name))
    //    .Where(t => t.Name.EndsWith("Repository"))
    //    .AsImplementedInterfaces();

    //注册仓储，所有IRepository接口到Repository的映射
    builder.RegisterGeneric(typeof(Repository<>))
        //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
        .As(typeof(IRepository<>))
        .InstancePerDependency();

    ////注册外部数据库仓储
    //builder.RegisterGeneric(typeof(ExtraRepository<>))
    //.As(typeof(IExtraRepository<>))
    //.InstancePerDependency();


});

var app = builder.Build();


// Configure the HTTP request pipeline.
#region 旧版swagger

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseExceptionHandleMiddleware();
#endregion


//使用JWT中间件

app.UseCors("MyAllowSpecificOrigins");
app.UseJWTMiddlewareExtension();

app.UseRouting();
//JWT相关
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
#region 新版swagger

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
