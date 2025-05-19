using CityStateSim.Core.EventBus.Interfaces;
using CityStateSim.Infrastructure.Events;
using CityStateSim.Infrastructure.Factories.Interfaces;
using CityStateSim.Infrastructure.Factories;
using Microsoft.Extensions.DependencyInjection;
using CityStateSim.Infrastructure.Config.Interfaces;
using CityStateSim.Infrastructure.Config;
using CityStateSim.Core.Commands;
using CityStateSim.Infrastructure.Commands;
using CityStateSim.Infrastructure.Input;
using CityStateSim.Core.Config;

namespace CityStateSim.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddConfiguration();
        services.AddWorldFactory();
        services.AddEventBuses();
        return services;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IConfigProvider, JsonConfigProvider>();

        // existing tiles.json load...
        services.AddSingleton(provider =>
            provider.GetRequiredService<IConfigProvider>()
                    .LoadConfig<List<TileDefinition>>("tiles.json")
        );

        // new resources.json load
        services.AddSingleton(provider =>
            provider.GetRequiredService<IConfigProvider>()
                    .LoadConfig<List<ResourceDefinition>>("resources.json")
        );

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
