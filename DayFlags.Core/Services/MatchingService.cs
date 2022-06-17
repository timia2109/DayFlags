using DayFlags.Core.MatchProvider;
using DayFlags.Core.Models;

namespace DayFlags.Core.Services;

/// <summary>
/// Provides query methods for satisfied days
/// </summary>
public class MatchingService
{

    private readonly IEnumerable<IMatchProvider> _matchProvider;

    public MatchingService(IEnumerable<IMatchProvider> matchProvider)
    {
        _matchProvider = matchProvider;
    }

    private async Task<List<TResults>> Run<TResults>(
        IEnumerable<IMatchProvider> matchProviders,
        Func<IMatchProvider, Task<TResults>> funcDelegate
    )
    {
        var results = new List<TResults>();

        var syncProviders = matchProviders.Where(e => e.RequireDatabase);
        var asyncProviders = matchProviders.Where(e => !e.RequireDatabase);

        // Start the async tasks
        var asyncTasks = asyncProviders.Select(e => funcDelegate(e));

        // Handle the sync tasks
        foreach (var provider in syncProviders)
        {
            var result = await funcDelegate(provider);
            results.Add(result);
        }

        // Collect the asyncs
        var asyncResults = await Task.WhenAll(asyncTasks);

        results.AddRange(asyncResults);

        return results;
    }

    private IEnumerable<IMatchProvider> MatchProvidersFor(EntryType entryType)
    {
        return _matchProvider.Where(e => e.IsResponsibleFor(entryType));
    }

    /// <summary>
    /// Return all Matches for the given <see cref="EntryType"/>
    /// </summary>
    /// <param name="entityType">Searched <see cref="EntryType"/></param>
    /// <returns>satisfied days</returns>
    public async Task<IList<DateOnly>> FindAllMatchesAsync(EntryType entityType)
    {
        var results = await Run(
            MatchProvidersFor(entityType),
            e => e.FindAllMatchesAsync(entityType));

        return results
            .SelectMany(e => e)
            .ToList();
    }

    /// <summary>
    /// Finds matches in Range for the given <see cref="EntryType"/>
    /// </summary>
    /// <param name="rangeBegin">Begin of range</param>
    /// <param name="rangeEnd">End of range</param>
    /// <param name="entityType">Searched <see cref="EntryType"/></param>
    /// <returns>satisfied days</returns>
    public async Task<IList<DateOnly>> FindMatchesAsync(DateOnly rangeBegin,
        DateOnly rangeEnd, EntryType entityType)
    {
        var results = await Run(
            MatchProvidersFor(entityType),
            e => e.FindMatchesAsync(rangeBegin, rangeEnd, entityType));

        return results
            .SelectMany(e => e)
            .ToList();
    }
}