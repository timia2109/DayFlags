using DayFlags.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core.Repositories;

internal class RealmRepository(DayFlagsDb db) : IRealmRepository
{
    public async Task<Realm> AddRealmAsync(Realm realm)
    {
        db.Realms.Add(realm);
        await db.SaveChangesAsync();
        return realm;
    }

    public async Task<Realm> DeleteRealmAsync(Realm realm)
    {
        db.Realms.Remove(realm);
        await db.SaveChangesAsync();
        return realm;
    }

    public async Task<Realm?> FindRealmAsync(Guid realmId)
    {
        return await db.Realms.FindAsync(realmId);
    }

    public IQueryable<Realm> GetRealmsQuery()
    {
        return db.Realms
            .OrderBy(r => r.Label);
    }

    public async Task<Realm> UpdateRealmAsync(Realm realm)
    {
        db.Entry(realm).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return realm;
    }
}