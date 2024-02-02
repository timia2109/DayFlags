using DayFlags.Core;
using DayFlags.Server.Middlewares;
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

// Init Db
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DayFlagsDb>();
    db.Database.EnsureCreated();
}

app.MapControllers();

app.UseMiddleware<RestExceptionMiddleware>();

app.Run();
