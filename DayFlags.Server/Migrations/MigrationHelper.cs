using DayFlags.Core;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Server.Migrations;

public partial class MigrationHelper(ILogger<MigrationHelper> logger,
    IServiceScopeFactory serviceScopeFactory)
{
    private readonly ILogger _logger = logger;

    public void ApplyMigrations()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<DayFlagsDb>();

        var migrations = database.Database.GetPendingMigrations()
            .ToArray();

        if (migrations.Length == 0)
        {
            LogNoMigration();
            return;
        }

        LogPendingMigrationsOverview(migrations.Length, migrations);
        database.Database.Migrate();
        LogFinishMigrating();
    }

    [LoggerMessage(LogLevel.Information, "No pending migration. Continue starting...")]
    private partial void LogNoMigration();

    [LoggerMessage(LogLevel.Information, "Found {migrationCount} pending migrations. Migrations: {migrations}")]
    private partial void LogPendingMigrationsOverview(int migrationCount, string[] migrations);

    [LoggerMessage(LogLevel.Information, "Applied all migration. Continue starting...")]
    private partial void LogFinishMigrating();
}