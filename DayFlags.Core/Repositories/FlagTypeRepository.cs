using DayFlags.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core.Repositories;

internal class FlagTypeRepository(DayFlagsDb db) : IFlagTypeRepository
{
    public async Task<FlagType> AddFlagTypeAsync(FlagType flagType)
    {
        db.FlagTypes.Add(flagType);
        await db.SaveChangesAsync();
        return flagType;
    }

    public async Task<FlagType> DeleteFlagTypeAsync(FlagType flagType)
    {
        db.FlagTypes.Remove(flagType);
        await db.SaveChangesAsync();
        return flagType;
    }

    public Task<FlagType?> GetFlagTypeAsync(Realm realm, string flagTypeKey)
    {
        return GetFlagTypesQuery(realm)
            .SingleOrDefaultAsync(f => f.FlagTypeKey == flagTypeKey);
    }

    public ValueTask<FlagType?> GetFlagTypeAsync(Guid flagTypeId)
    {
        return db.FlagTypes.FindAsync(flagTypeId);
    }

    public IQueryable<FlagType> GetFlagTypesQuery(Realm realm)
    {
        return db.FlagTypes
            .Where(f => f.Realm == realm);
    }

    public async Task<FlagType> UpdateFlagTypeAsync(FlagType flagType)
    {
        db.Entry(flagType).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return flagType;
    }
}