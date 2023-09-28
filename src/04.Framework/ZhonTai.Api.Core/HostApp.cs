using AspNetCoreRateLimit;
using Autofac;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Mapster;
using FluentValidation;
using FluentValidation.AspNetCore;
using ZhonTai.Api.Core.Auth;
using ZhonTai.Utils.Helpers;
using ZhonTai.Api.Core.Db;
using ZhonTai.Api.Core.Extensions;
using ZhonTai.Api.Core.Filters;
using ZhonTai.Api.Core.RegisterModules;
using Microsoft.OpenApi.Any;
using Microsoft.AspNetCore.Mvc.Controllers;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Api.Core.Consts;
using MapsterMapper;
using ZhonTai.Api.DynamicApi;
using NLog.Web;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Startup;
using ZhonTai.Api.Core.Conventions;
using FreeSql;
using ZhonTai.Api.Core.Middlewares;
using ZhonTai.Api.DynamicApi.Attributes;
using System.Text.RegularExpressions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;
using FreeRedis;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;
using NLog;
using Microsoft.Extensions.Logging;
using ZhonTai.Api.Core.Handlers;
using ZhonTai.Plugin.Cache;
using ProtoBuf.Grpc.Server;
using ZhonTai.Api.Core.GrpcServices;
using ZhonTai.Api.Rpc.Configs;
using ZhonTai.Api.Core.IdGenerator;
using ZhonTai.Api.Rpc.Dtos;
using ZhonTai.Api.Rpc.Helpers;

namespace ZhonTai.Api.Core;

/// <summary>
/// 宿主应用
/// </summary>
public class HostApp
{
    readonly HostAppOptions _hostAppOptions;

    public HostApp()
    {
    }

    public HostApp(HostAppOptions hostAppOptions)
    {
        _hostAppOptions = hostAppOptions;
    }

    /// <summary>
    /// 添加配置文件
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="environmentName">环境名</param>
    /// <param name="directory">目录</param>
    /// <param name="optional">可选</param>
    /// <param name="reloadOnChange">热更新</param>
    private static void AddJsonFilesFromDirectory(
        ConfigurationManager configuration,
        string environmentName,
        string directory = "Configs",
        bool optional = true,
        bool reloadOnChange = true)
    { 
        var allFilePaths = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, directory).ToPath())
            .Where(p => p.EndsWith($".json", StringComparison.OrdinalIgnoreCase));

        var environmentFilePaths = allFilePaths.Where(p => p.EndsWith($".{environmentName}.json", StringComparison.OrdinalIgnoreCase));
        var otherFilePaths = allFilePaths.Except(environmentFilePaths);
        var filePaths = otherFilePaths.Concat(environmentFilePaths);

        foreach (var filePath in filePaths)
        {
            configuration.AddJsonFile(filePath, optional: optional, reloadOnChange: reloadOnChange);
        }
    }

    /// <summary>
    /// 运行应用
    /// </summary>
    /// <param name="args"></param>
    public void Run(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        try
        {
            //应用程序启动
            logger.Info("Application startup");

            var builder = WebApplication.CreateBuilder(args);
            _hostAppOptions?.ConfigurePreWebApplicationBuilder?.Invoke(builder);

            builder.ConfigureApplication(Assembly.GetCallingAssembly());
            //清空日志供应程序，避免.net自带日志输出到命令台
            builder.Logging.ClearProviders();
            //使用NLog日志
            builder.Host.UseNLog();

            var services = builder.Services;
            var env = builder.Environment;
            var configuration = builder.Configuration;

            //添加配置
            AddJsonFilesFromDirectory(configuration, env.EnvironmentName);

            configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (env.EnvironmentName.NotNull())
            {
                configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            var configHelper = new ConfigHelper();

            services.Configure<AppConfig>(configuration.GetSection("ApiConfig:AppConfig"));
            services.Configure<JwtConfig>(configuration.GetSection("ApiConfig:JwtConfig"));
            services.Configure<DbConfig>(configuration.GetSection("ApiConfig:DbConfig"));
            services.Configure<CacheConfig>(configuration.GetSection("ApiConfig:CacheConfig"));
            services.Configure<RpcConfig>(configuration.GetSection("ApiConfig:RpcConfig"));

            //应用配置
            var appConfig = AppInfo.GetOptions<AppConfig>();
            services.AddSingleton(appConfig);

            //数据库配置
            var dbConfig = AppInfo.GetOptions<DbConfig>();
            services.AddSingleton(dbConfig);

            var hostAppContext = new HostAppContext()
            {
                Services = services,
                Environment = env,
                Configuration = configuration
            };

            //使用Autofac容器
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            //配置Autofac容器
            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                // 控制器注入
                builder.RegisterModule(new ControllerModule());

                // 单例注入
                builder.RegisterModule(new SingleInstanceModule(appConfig));

                // 模块注入
                builder.RegisterModule(new RegisterModule(appConfig));

                _hostAppOptions?.ConfigureAutofacContainer?.Invoke(builder, hostAppContext);
            });

            //配置Kestrel服务器
            builder.WebHost.ConfigureKestrel((context, options) =>
            {
                //设置应用服务器Kestrel最大请求大小
                options.Limits.MaxRequestBodySize = appConfig.MaxRequestBodySize;
            });

            //配置服务
            ConfigureServices(services, env, configuration, configHelper, appConfig);

            _hostAppOptions?.ConfigureWebApplicationBuilder?.Invoke(builder);

            var app = builder.Build();

            app.ConfigureApplication();

            app.Lifetime.ApplicationStarted.Register(() =>
            {
                AppInfo.IsRun = true;
            });

            app.Lifetime.ApplicationStopped.Register(() =>
            {
                AppInfo.IsRun = false;
            });

            //配置中间件
            ConfigureMiddleware(app, env, configuration, appConfig);

            app.Run();

            //应用程序停止
            logger.Info("Application shutdown");
        }
        catch (Exception exception)
        {
            //应用程序异常
            logger.Error(exception, "Application stopped because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    /// <param name="configuration"></param>
    /// <param name="configHelper"></param>
    /// <param name="appConfig"></param>
    private void ConfigureServices(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration, ConfigHelper configHelper, AppConfig appConfig)
    {
        var hostAppContext = new HostAppContext()
        {
            Services = services,
            Environment = env,
            Configuration = configuration
        };

        _hostAppOptions?.ConfigurePreServices?.Invoke(hostAppContext);

        //健康检查
        services.AddHealthChecks();
        
        //权限处理
        services.AddScoped<IPermissionHandler, PermissionHandler>();

        // ClaimType不被更改
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        //用户信息
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.TryAddScoped<IUser, User>();

        //添加数据库
        if (!_hostAppOptions.CustomInitDb)
        {
            services.AddDb(env, _hostAppOptions);
        }

        //程序集
        Assembly[] assemblies = AssemblyHelper.GetAssemblyList(appConfig.AssemblyNames);

        #region Mapster 映射配置
        services.AddScoped<IMapper>(sp => new Mapper());
        if(assemblies?.Length > 0)
        {
            TypeAdapterConfig.GlobalSettings.Scan(assemblies);
        }
        #endregion Mapster 映射配置

        #region Cors 跨域
        services.AddCors(options =>
        {
            options.AddPolicy(AdminConsts.RequestPolicyName, policy =>
            {
                var hasOrigins = appConfig.CorUrls?.Length > 0;
                if (hasOrigins)
                {
                    policy.WithOrigins(appConfig.CorUrls);
                }
                else
                {
                    policy.AllowAnyOrigin();
                }
                policy
                .AllowAnyHeader()
                .AllowAnyMethod();

                if (hasOrigins)
                {
                    policy.AllowCredentials();
                }
            });

            //允许任何源访问Api策略，使用时在控制器或者接口上增加特性[EnableCors(AdminConsts.AllowAnyPolicyName)]
            options.AddPolicy(AdminConsts.AllowAnyPolicyName, policy =>
            {
                policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

        #endregion Cors 跨域

        #region 身份认证授权
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = appConfig.IdentityServer.Enable ? IdentityServerAuthenticationDefaults.AuthenticationScheme : JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
            options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
        })
        .AddJwtBearer(options =>
        {
            //ids4
            if (appConfig.IdentityServer.Enable)
            {
                options.Authority = appConfig.IdentityServer.Url;
                options.RequireHttpsMetadata = appConfig.IdentityServer.RequireHttpsMetadata;
                options.Audience = appConfig.IdentityServer.Audience;
            }
            else
            {
                var jwtConfig = AppInfo.GetOptions<JwtConfig>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtConfig.ValidateIssuer,
                    ValidIssuer = jwtConfig.ValidIssuer,
                    ValidateAudience = jwtConfig.ValidateAudience,
                    ValidAudience = jwtConfig.ValidAudience,
                    ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.IssuerSigningKey)),
                    ValidateLifetime = jwtConfig.ValidateLifetime,
                    RequireExpirationTime = jwtConfig.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromSeconds(jwtConfig.ClockSkew)
                };
            }
        })
        .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });

        #endregion 身份认证授权

        #region Swagger Api文档

        if (env.IsDevelopment() || appConfig.Swagger.Enable)
        {
            services.AddSwaggerGen(options =>
            {
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerDoc(project.Code.ToLower(), project);
                });

                options.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    var api = controllerAction.AttributeRouteInfo.Template;
                    api = Regex.Replace(api, @"[\{\\\/\}]", "-") + "-" + apiDesc.HttpMethod.ToLower();
                    return api.Replace("--", "-");
                });

                options.ResolveConflictingActions(apiDescription => apiDescription.First());

                string DefaultSchemaIdSelector(Type modelType)
                {
                    var modelName = modelType.Name;
                    if (appConfig.Swagger.EnableSchemaIdNamespace)
                    {
                        var nameSpaceList = appConfig.Swagger.AssemblyNameList;
                        if (nameSpaceList?.Length > 0)
                        {
                            var nameSpace = modelType.Namespace;
                            if(nameSpaceList.Where(a => nameSpace.Contains(a)).Any())
                            {
                                modelName = modelType.FullName;
                            }
                        }
                        else
                        {
                            modelName = modelType.FullName;
                        }
                    }

                    if (modelType.IsConstructedGenericType)
                    {
                        var prefix = modelType.GetGenericArguments()
                        .Select(DefaultSchemaIdSelector)
                        .Aggregate((previous, current) => previous + current);

                        modelName = modelName.Split('`').First() + prefix;
                    }
                    else
                    {
                        modelName = modelName.Replace("[]", "Array");
                    }

                    if (modelType.IsDefined(typeof(SchemaIdAttribute)))
                    {
                        var swaggerSchemaIdAttribute = modelType.GetCustomAttribute<SchemaIdAttribute>(false);
                        if (swaggerSchemaIdAttribute.SchemaId.NotNull())
                        {
                            return swaggerSchemaIdAttribute.SchemaId;
                        }
                        else
                        {
                            return swaggerSchemaIdAttribute.Prefix + modelName + swaggerSchemaIdAttribute.Suffix;
                        }
                    }

                    return modelName;
                }

                options.CustomSchemaIds(modelType => DefaultSchemaIdSelector(modelType));

                //支持多分组
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    var nonGroup = false;
                    var groupNames = new List<string>();
                    var dynamicApiAttribute = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is DynamicApiAttribute);
                    if (dynamicApiAttribute != null)
                    {
                        var dynamicApi = dynamicApiAttribute as DynamicApiAttribute;
                        if(dynamicApi.GroupNames?.Length > 0)
                        {
                            groupNames.AddRange(dynamicApi.GroupNames);
                        }
                    }

                    var apiGroupAttribute = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is ApiGroupAttribute);
                    if (apiGroupAttribute != null)
                    {
                        var apiGroup = apiGroupAttribute as ApiGroupAttribute;
                        if (apiGroup.GroupNames?.Length > 0)
                        {
                            groupNames.AddRange(apiGroup.GroupNames);
                        }
                        nonGroup = apiGroup.NonGroup;
                    }

                    return docName == apiDescription.GroupName || groupNames.Any(a => a == docName) || nonGroup;
                });

                string[] xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                if (xmlFiles.Length > 0)
                {
                    foreach (var xmlFile in xmlFiles)
                    {
                        options.IncludeXmlComments(xmlFile, true);
                    }
                }

                var server = new OpenApiServer()
                {
                    Url = appConfig.Swagger.Url,
                    Description = ""
                };
                if (appConfig.ApiUI.Footer.Enable)
                {
                    server.Extensions.Add("extensions", new OpenApiObject
                    {
                        ["footer"] = new OpenApiObject
                        {
                            ["enable"] = new OpenApiBoolean(appConfig.ApiUI.Footer.Enable),
                            ["content"] = new OpenApiString(appConfig.ApiUI.Footer.Content)
                        }
                    });
                }
                options.AddServer(server);

                if(appConfig.Swagger.EnableEnumSchemaFilter)
                {
                    options.SchemaFilter<EnumSchemaFilter>();
                }
                if(appConfig.Swagger.EnableOrderTagsDocumentFilter)
                {
                    options.DocumentFilter<OrderTagsDocumentFilter>();
                }
                options.OrderActionsBy(apiDesc =>
                {
                    var order = 0;
                    var objOrderAttribute = apiDesc.CustomAttributes().FirstOrDefault(x => x is OrderAttribute);
                    if (objOrderAttribute != null)
                    {
                        var orderAttribute = objOrderAttribute as OrderAttribute;
                        order = orderAttribute.Value;
                    }
                    return (int.MaxValue - order).ToString().PadLeft(int.MaxValue.ToString().Length, '0');
                });

                #region 添加设置Token的按钮

                if (appConfig.IdentityServer.Enable)
                {
                    //添加Jwt验证设置
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "oauth2",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                    //统一认证
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Description = "oauth2登录授权",
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{appConfig.IdentityServer.Url}/connect/authorize", UriKind.Absolute),
                                TokenUrl = new Uri($"{appConfig.IdentityServer.Url}/connect/token", UriKind.Absolute),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "admin.server.api", "admin后端api" }
                                }
                            }
                        }
                    });
                }
                else
                {
                    //添加Jwt验证设置
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Value: Bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                }

                #endregion 添加设置Token的按钮
            });
        }

        #endregion Swagger Api文档

        #region 操作日志

        if (appConfig.Log.Operation)
        {
            services.AddScoped<ILogHandler, LogHandler>();
        }

        #endregion 操作日志

        #region 控制器
        void mvcConfigure(MvcOptions options)
        {
            //options.Filters.Add<ControllerExceptionFilter>();
            options.Filters.Add<ValidateInputFilter>();
            if (appConfig.Validate.Login || appConfig.Validate.Permission)
            {
                options.Filters.Add<ValidatePermissionAttribute>();
            }
            //在具有较高的 Order 值的筛选器之前运行 before 代码
            //在具有较高的 Order 值的筛选器之后运行 after 代码
            if (appConfig.DynamicApi.FormatResult)
            {
                options.Filters.Add<FormatResultFilter>(20);
            }
            if (appConfig.Log.Operation)
            {
                options.Filters.Add<ControllerLogFilter>(10);
            }

            //禁止去除ActionAsync后缀
            //options.SuppressAsyncSuffixInActionNames = false;

            if (env.IsDevelopment() || appConfig.Swagger.Enable)
            {
                //API分组约定
                options.Conventions.Add(new ApiGroupConvention());
            }
        }

        var mvcBuilder = appConfig.AppType switch
        {
            AppType.Controllers => services.AddControllers(mvcConfigure),
            AppType.ControllersWithViews => services.AddControllersWithViews(mvcConfigure),
            AppType.MVC => services.AddMvc(mvcConfigure),
            _ => services.AddControllers(mvcConfigure)
        };

        if (assemblies?.Length > 0)
        {
            foreach (var assembly in assemblies)
            {
                services.AddValidatorsFromAssembly(assembly);
            }
        }
        services.AddFluentValidationAutoValidation();

        mvcBuilder.AddNewtonsoftJson(options =>
        {
            //忽略循环引用
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //使用驼峰 首字母小写
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //设置时间格式
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss.FFFFFFFK";
        })
        .AddControllersAsServices();

        if (appConfig.Swagger.EnableJsonStringEnumConverter)
            mvcBuilder.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        _hostAppOptions?.ConfigureMvcBuilder?.Invoke(mvcBuilder, hostAppContext);
        #endregion 控制器

        services.AddHttpClient();

        _hostAppOptions?.ConfigureServices?.Invoke(hostAppContext);

        #region 缓存
        //添加内存缓存
        services.AddMemoryCache();

        var cacheConfig = AppInfo.GetOptions<CacheConfig>();
        if (cacheConfig.Type == CacheType.Redis)
        {
            //FreeRedis客户端
            services.AddSingleton<IRedisClient>(serviceProvider =>
            {
                return new RedisClient(cacheConfig.ConnectionString)
                {
                    Serialize = JsonConvert.SerializeObject,
                    Deserialize = JsonConvert.DeserializeObject
                };
            });
            //Redis缓存
            services.AddSingleton<ICache, RedisCache>();
            //分布式Redis缓存
            services.AddSingleton<IDistributedCache>(serviceProvider  => {
                var redisClient = new RedisClient(cacheConfig.ConnectionString)
                {
                    Serialize = JsonConvert.SerializeObject,
                    Deserialize = JsonConvert.DeserializeObject
                };
                return new DistributedCache(redisClient);
            });
        }
        else
        {
            //内存缓存
            services.AddSingleton<ICache, MemoryCache>();
            //分布式内存缓存
            services.AddDistributedMemoryCache();
        }

        #endregion 缓存

        services.AddSingleton<WorkerNode>().AddHostedService<WorkerNodeHostedService>();

        #region IP限流

        if (appConfig.RateLimit)
        {
            services.AddMyIpRateLimit();
        }

        #endregion IP限流

        //阻止NLog接收状态消息
        services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

        //性能分析
        if (appConfig.MiniProfiler)
        {
            services.AddMiniProfiler();
        }

        //动态api
        services.AddDynamicApi(options =>
        {
            options.FormatResult = appConfig.DynamicApi.FormatResult;
            options.FormatResultType = typeof(Response<>);

            _hostAppOptions?.ConfigureDynamicApi?.Invoke(options);
        });

        services.AddCodeFirstGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            //options.ResponseCompressionLevel = CompressionLevel.Optimal;
        });
        services.AddCodeFirstGrpcReflection();

        var policies = PolicyHelper.GetPolicyList();
        services.AddMyGrpcClients(AppInfo.EffectiveAssemblies, AppInfo.GetOptions<RpcConfig>(), policies);

        _hostAppOptions?.ConfigurePostServices?.Invoke(hostAppContext);
    }

    /// <summary>
    /// 配置中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="configuration"></param>
    /// <param name="appConfig"></param>
    private void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env, IConfiguration configuration, AppConfig appConfig)
    {
        var hostAppMiddlewareContext = new HostAppMiddlewareContext()
        {
            App = app,
            Environment = env,
            Configuration = configuration
        };

        _hostAppOptions?.ConfigurePreMiddleware?.Invoke(hostAppMiddlewareContext);

        //异常处理
        app.UseMiddleware<ExceptionMiddleware>();

        //IP限流
        if (appConfig.RateLimit)
        {
            app.UseIpRateLimiting();
        }

        //性能分析
        if (appConfig.MiniProfiler)
        {
            app.UseMiniProfiler();
        }

        //静态文件
        app.UseDefaultFiles();
        app.UseStaticFiles();

        //路由
        app.UseRouting();

        //跨域
        app.UseCors(AdminConsts.RequestPolicyName);

        //认证
        app.UseAuthentication();

        //授权
        app.UseAuthorization();

        //登录用户初始化数据权限
        if (appConfig.Validate.DataPermission)
        {
            app.Use(async (ctx, next) =>
            {
                var user = ctx.RequestServices.GetRequiredService<IUser>();
                if (user?.Id > 0)
                {
                    var userGrpcService = ctx.RequestServices.GetRequiredService<IUserGrpcService>();
                    await userGrpcService.GetDataPermissionAsync();
                }

                await next();
            });
        }

        //配置端点
        app.MapControllers();

        _hostAppOptions?.ConfigureMiddleware?.Invoke(hostAppMiddlewareContext);

        #region Swagger Api文档
        if (env.IsDevelopment() || appConfig.Swagger.Enable)
        {
            var routePrefix = appConfig.ApiUI.RoutePrefix;
            if (!appConfig.ApiUI.Enable && routePrefix.IsNull())
            {
                routePrefix = appConfig.Swagger.RoutePrefix;
            }
            var routePath = routePrefix.NotNull() ? $"{routePrefix}/" : "";
            app.UseSwagger(optoins =>
            {
                optoins.RouteTemplate = routePath + optoins.RouteTemplate;
            });
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = appConfig.Swagger.RoutePrefix;
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{routePath}swagger/{project.Code.ToLower()}/swagger.json", project.Title);
                });
                
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                //options.DefaultModelsExpandDepth(-1);//不显示Models
                if (appConfig.MiniProfiler)
                {
                    options.InjectJavascript("/swagger/mini-profiler.js?v=4.2.22+2.0");
                    options.InjectStylesheet("/swagger/mini-profiler.css?v=4.2.22+2.0");
                }
            });
        }
        #endregion Swagger Api文档

        //使用健康检查
        if(appConfig.HealthChecks.Enable)
        {
            app.MapHealthChecks(appConfig.HealthChecks.Path, new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }

        app.UseMyMapGrpcService(AppInfo.EffectiveAssemblies);

        app.MapCodeFirstGrpcReflectionService();

        _hostAppOptions?.ConfigurePostMiddleware?.Invoke(hostAppMiddlewareContext);
    }
}