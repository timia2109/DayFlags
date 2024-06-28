using DayFlags.Core.Database.Models;
using DayFlags.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core.Database;

/// <summary>
/// Database Context
/// </summary>
public class DayFlagsDb(DbContextOptions options) : DbContext(options)
{
    public DbSet<DbDayFlag> DayFlags => Set<DbDayFlag>();
    public DbSet<DbFlagGroup> FlagGroups => Set<DbFlagGroup>();
    public DbSet<DbFlagType> FlagTypes => Set<DbFlagType>();
    public DbSet<Realm> Realms => Set<Realm>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DayFlag>()
            .HasIndex(e => e.Date);

        modelBuilder.Entity<FlagGroup>()
           .HasIndex(e => e.FlagGroupKey);

        modelBuilder.Entity<FlagType>()
            .HasIndex(e => e.FlagTypeKey);
    }

}