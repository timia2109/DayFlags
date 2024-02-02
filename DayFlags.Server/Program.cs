using DayFlags;
using DayFlags.Server.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<DayFlagsDb>(options =>
{
    options.UseSqlite("Data Source=DayFlags.db;");
});
builder.Services
    .AddControllers();

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
