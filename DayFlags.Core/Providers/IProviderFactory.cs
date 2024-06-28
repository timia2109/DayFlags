using DayFlags.Core.Models;

namespace DayFlags.Core.Providers;

/// <summary>
/// Factory for an Provider
/// </summary>
public interface IProviderFactory
{
    /// <summary>
    /// Type of the Settings
    /// </summary>
    public Type SettingsType { get; }
    
    /// <summary>
    /// Returns the Id for an Provider
    /// </summary>
    public string ProviderId { get; }

    /// <summary>
    /// Builds the <see cref="IDayFlagProvider"/> for this request scope
    /// </summary>
    /// <param name="realm">Realm</param>
    /// <param name="configuration">Configuration (of type SettingsType)</param>
    /// <returns>A DayFlag Provider (if any)</returns>
    public IDayFlagProvider? BuildDayFlagsProvider(Realm realm, dynamic configuration);
}