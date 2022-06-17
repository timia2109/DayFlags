using DayFlags.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DayFlags;

public class DayFlagsDb : DbContext
{

    public DayFlagsDb(DbContextOptions options) : base(options) { }

    public DbSet<DayEntry> DayEntries => Set<DayEntry>();
    public DbSet<EntryType> EntryTypes => Set<EntryType>();

}