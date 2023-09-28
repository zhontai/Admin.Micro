using System.Reflection;
using ZhonTai.Utils.Extensions;

namespace ZhonTai.Api.Core;

public class ModuleInfo
{
    private static ModuleInfo? _instance = null;
    private static readonly object _lockObj = new();

    /// <summary>
    /// 模块唯一Id
    /// </summary>
    public string Id { get; private set; } = string.Empty;
    /// <summary>
    /// 模块名称
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    /// <summary>
    /// 模块短命名
    /// </summary>
    public string ShortName { get; private set; } = string.Empty;
    /// <summary>
    /// 模块全命名
    /// </summary>
    public string FullName { get; private set; } = string.Empty;
    /// <summary>
    /// 模块版本号
    /// </summary>
    public string Version { get; private set; } = string.Empty;
    /// <summary>
    /// 模块描述
    /// </summary>
    public string Description { get; private set; } = string.Empty;
    public Assembly Assembly { get; private set; } = default!;

    private ModuleInfo()
    {
    }

    public static ModuleInfo CreateInstance(Assembly assembly)
    {
        if (_instance is not null)
            return _instance;

        lock (_lockObj)
        {
            if (_instance is not null)
                return _instance;

            if (assembly is null)
                assembly = Assembly.GetEntryAssembly() ?? throw new NullReferenceException(nameof(assembly));

            var attribute = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            var description = attribute is null ? string.Empty : attribute.Description;
            var version = assembly.GetName().Version ?? throw new NullReferenceException("startAssembly.GetName().Version");
            var assemblyName = assembly.GetName().Name ?? string.Empty;
            var serviceName = assemblyName.Replace(".", "-").ToLower();
            var ticks = DateTime.Now.ToTimestamp(true);
            var ticksHex = Convert.ToString(ticks, 16);
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();
            var serviceId = envName switch
            {
                "development" => $"{serviceName}-dev-{ticksHex}",
                "test" => $"{serviceName}-test-{ticksHex}",
                "staging" => $"{serviceName}-stag-{ticksHex}",
                "production" => $"{serviceName}-prod-{ticksHex}",
                _ => $"{serviceName}-{envName}-{ticksHex}",
            };

            var assemblyNames = assemblyName.Split(".");
            _instance = new ModuleInfo
            {
                Id = serviceId,
                Name = assemblyNames[^2].ToLower(),
                FullName = serviceName,
                ShortName = $"{assemblyNames[^2]}-{assemblyNames[^1]}".ToLower(),
                Assembly = assembly,
                Description = description,
                Version = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}"
            };
        }
        return _instance;
    }

    public static ModuleInfo GetInstance()
    {
        if (_instance is null)
            throw new NullReferenceException(nameof(ModuleInfo));

        return _instance;
    }
}
