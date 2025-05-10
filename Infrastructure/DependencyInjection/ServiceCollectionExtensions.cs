using Core.EventBus.Interfaces;
using Infrastructure.Events;
using Infrastructure.Factories.Interfaces;
using Infrastructure.Factories;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Config.Interfaces;
using Infrastructure.Config;

namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddConfiguration();
        services.AddWorldFactory();
        services.AddEventBuses();
        return services;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IConfigProvider, JsonConfigProvider>();

        return services;
    }

    public static IServiceCollection AddWorldFactory(this IServiceCollection services)
    {
        services.AddSingleton<IWorldFactory, DefaultEcsWorldFactory>();
        return services;
    }

    public static IServiceCollection AddEventBuses(this IServiceCollection services)
    {
        services.AddSingleton<IAppEventBus, InMemoryAppEventBus>();

        services.AddScoped(sp =>
            sp.GetRequiredService<IWorldFactory>().CreateWorld());

        services.AddScoped<IEcsEventBus, EcsEventBusAdapter>();

        return services;
    }
}
