using DayFlags.Core.Models;

namespace DayFlags.Core.Repositories;

/// <summary>
/// Repository for <see cref="FlagType"/>s
/// </summary>
public interface IFlagTypeRepository
{
    Task<IEnumerable<FlagType>> GetFlagTypesAsync(Realm realm);
    Task<FlagType?> GetFlagTypeAsync(Realm realm, string flagTypeKey);
    Task<FlagType?> GetFlagTypeAsync(Guid flagTypeId);
    Task<FlagType> AddFlagTypeAsync(FlagType flagType);
    Task<FlagType> UpdateFlagTypeAsync(FlagType flagType);
    Task<FlagType> DeleteFlagTypeAsync(FlagType flagType);
}