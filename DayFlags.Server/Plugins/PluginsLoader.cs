using DayFlags.Core.Plugins;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Logging.Console;

namespace DayFlags.Server.Plugins;

/// <summary>
/// Responsible to load Plugins
/// </summary>
public class PluginsLoader
{

    private readonly static Type[] SharedTypes = new Type[]{
        typeof(IServiceCollection), typeof(IServiceProvider), typeof(ILogger),
        typeof(IDayFlagPlugin)
    };

    private readonly IConfiguration _config;
    private List<IDayFlagPlugin> _plugins = new();
    private readonly LoggerFactory _loggerFactory;
    private readonly ILogger _logger;

    public PluginsLoader(IConfiguration config)
    {
        _config = config;
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new ConsoleLoggerProvider(null));
        _logger = _loggerFactory.CreateLogger("PluginLoader");
    }

    private void LoadPlugin(string assemblyPath)
    {
        _logger.LogInformation($"Loading Plugin {assemblyPath}");
        var plugin = PluginLoader.CreateFromAssemblyFile(
            assemblyPath, true, SharedTypes
        );

        if (plugin != null)
        {
            var pluginEntries = plugin.LoadDefaultAssembly()
                .GetTypes()
                .Where(e => e.IsAssignableFrom(typeof(IDayFlagPlugin))
                    && !e.IsAbstract);

            foreach (var pluginEntry in pluginEntries)
            {
                var instance = (IDayFlagPlugin?)Activator
                    .CreateInstance(pluginEntry);

                if (instance != null)
                {
                    _plugins.Add(instance);
                }
            }
        }
    }

    public void ConfigureServices(IServiceCollection serviceDescriptors)
    {
        foreach (var plugin in _plugins)
        {
            serviceDescriptors.AddSingleton(typeof(IDayFlagPlugin), plugin);
            plugin.ConfigureServices(serviceDescriptors);
        }
    }

    public void ConfigureApp(WebApplicationBuilder webApplicationBuilder)
    {
        foreach (var plugin in _plugins)
        {
            plugin.ConfigureApp(webApplicationBuilder);
        }
    }
}