using DayFlags;
using DayFlags.Core.EntryTypes;
using DayFlags.Core.MatchProvider;
using DayFlags.Server.Converters;
using DayFlags.Server.EntryTypes;
using DayFlags.Server.MatchProvider;
using DayFlags.Server.Middlewares;
using DayFlags.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<EntryTypeService>();
builder.Services.AddScoped<DayEntriesService>();
builder.Services.AddScoped<MatchingService>();
builder.Services.AddScoped<IMatchProvider, DatabaseMatchProvider>();
builder.Services.AddScoped<AEntryTypeProvider, DbEntryTypesProvider>();

builder.Services.AddDbContext<DayFlagsDb>(options =>
{
    options.UseSqlite("Data Source=DayFlags.db;");
});
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new DateOnlyConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

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
