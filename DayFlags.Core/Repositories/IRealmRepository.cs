using DayFlags.Core.Models;

namespace DayFlags.Core.Repositories;

/// <summary>
/// Manages the Realms
/// </summary>
public interface IRealmRepository
{
    IQueryable<Realm> GetRealmsQuery();
    Task<Realm?> FindRealmAsync(Guid realmId);
    Task<Realm> AddRealmAsync(Realm realm);
    Task<Realm> UpdateRealmAsync(Realm realm);
    Task<Realm> DeleteRealmAsync(Realm realm);
}