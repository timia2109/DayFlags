using DayFlags.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DayFlags.Core;

public static class ServiceCollectionExtension
{

    public static IServiceCollection AddDayFlagsCore(this IServiceCollection services)
    {
        services.TryAddScoped<IDayFlagRepository, DayFlagRepository>();
        services.TryAddScoped<IFlagGroupRepository, FlagGroupRepository>();
        services.TryAddScoped<IFlagTypeRepository, FlagTypeRepository>();
        services.TryAddScoped<IRealmRepository, RealmRepository>();

        return services;
    }

}