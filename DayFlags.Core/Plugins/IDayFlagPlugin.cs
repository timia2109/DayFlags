using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DayFlags.Core.Plugins;

/// <summary>
/// Generic Interface to create Plugins
/// </summary>
public interface IDayFlagPlugin
{
    /// <summary>
    /// Provides Metadata about this plugin
    /// </summary>
    public PluginMetadata Metadata { get; }

    /// <summary>
    /// Configures Services and injecting DI Objects
    /// </summary>
    /// <param name="serviceDescriptors">Services Builder</param>
    public void ConfigureServices(IServiceCollection serviceDescriptors);

    /// <summary>
    /// Configures the Web App
    /// </summary>
    /// <param name="webApplicationBuilder">Web Application Builder</param>
    public void ConfigureApp(WebApplicationBuilder webApplicationBuilder);

    /// <summary>
    /// Gets called when this plugin was freshly installed.
    /// Gets called, after all Services are constructed
    /// </summary>
    public void OnInstall(IServiceProvider serviceProvider);

    /// <summary>
    /// Gets called when this plugin was uninstalled
    /// </summary>
    public void OnUninstall(IServiceProvider serviceProvider);

}