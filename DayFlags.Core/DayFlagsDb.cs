using DayFlags.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core;

/// <summary>
/// Database Context
/// </summary>
public class DayFlagsDb : DbContext
{

    public DayFlagsDb(DbContextOptions options) : base(options) { }

    public DbSet<DayFlag> DayFlags => Set<DayFlag>();
    public DbSet<FlagGroup> FlagGroups => Set<FlagGroup>();
    public DbSet<FlagType> FlagTypes => Set<FlagType>();
    public DbSet<Realm> Realms => Set<Realm>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DayFlag>()
            .HasKey(e => e.Date);

        modelBuilder.Entity<FlagGroup>()
           .HasKey(e => e.FlagGroupKey);

        modelBuilder.Entity<FlagType>()
            .HasKey(e => e.FlagTypeKey);
    }

}