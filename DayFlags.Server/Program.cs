using DayFlags.Core;
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
        ?? throw new ArgumentException("Postgres connection string is missing")
    );
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDayFlagSwagger();

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

// Init Db
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DayFlagsDb>();
    db.Database.EnsureCreated();
}

app.MapControllers();

app.Run();
