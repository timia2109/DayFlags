using DayFlags.Core;
using DayFlags.Server.Migrations;
using DayFlags.Server.Services;
using DayFlags.Server.Utils;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add local configuration
builder.Configuration.AddJsonFile("appsettings.Local.json", true);

// Add services to the container.
builder.Services.AddDayFlagsCore();

builder.Services.AddDbContext<DayFlagsDb>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres")
        ?? throw new ArgumentException("Postgres connection string is missing"),
        o =>
        {
            o.MigrationsAssembly("DayFlags.Server");
        }
    );
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDayFlagSwagger();
builder.Services.AddScoped<DayFlagApiService>();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is ResultException res)
        {
            await res.HandleAsync(context);
        }
    });
});

app.UseDayFlagSwagger();
app.UseAuthorization();

// Migrate DB
ActivatorUtilities.CreateInstance<MigrationHelper>(app.Services)
    .ApplyMigrations();

app.MapControllers();
app.MapWhen(x =>
{
    var path = x.Request.Path.Value;
    if (path is null) return true;
    return !path.StartsWith("/api");
},
    o =>
        o.UseSpa(s =>
        {
            if (app.Environment.IsDevelopment())
            {
                s.UseProxyToSpaDevelopmentServer("http://localhost:5173");
            }
        })
);

app.UseStaticFiles();
app.Run();
