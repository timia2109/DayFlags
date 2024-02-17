using Microsoft.OpenApi.Models;

namespace DayFlags.Server.Utils;

public static class SwaggerExtensions
{
    public static void AddDayFlagSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "DayFlags API - v1",
                    Version = "v1"
                }
            );
            c.SupportNonNullableReferenceTypes();

            var filePath = Path.Combine(AppContext.BaseDirectory, "DayFlags.Server.xml");
            c.IncludeXmlComments(filePath);
        });
    }

    public static void UseDayFlagSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseReDoc(c =>
        {
            c.RoutePrefix = "docs";
        });
    }

}