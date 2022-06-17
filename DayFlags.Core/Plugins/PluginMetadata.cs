namespace DayFlags.Core.Plugins;

/// <summary>
/// Provides metadata about this plugin
/// </summary>
public readonly record struct PluginMetadata(
    string PluginId,
    Uri RepositoryUri,
    string? Description
);