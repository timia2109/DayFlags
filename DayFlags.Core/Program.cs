using DayFlags;
using DayFlags.Core.Converters;
using DayFlags.Core.Exceptions;
using DayFlags.Core.Middlewares;
using DayFlags.Core.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<EntryTypeService>();
builder.Services.AddScoped<DayEntriesService>();

builder.Services.AddDbContext<DayFlagsDb>(options =>
{
    options.UseSqlite("Data Source=DayFlags.db;");
});
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => {
        options.SerializerSettings.Converters.Add(new DateOnlyConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
