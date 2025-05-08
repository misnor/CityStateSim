using Core.EventBus.Interfaces;
using Infrastructure.Events;
using Infrastructure.Factories.Interfaces;
using Infrastructure.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;
public static class ServiceCollectionExtensions
{
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

    public static IServiceCollection AddSimulation(this IServiceCollection services)
    {
        return services;
    }
}
