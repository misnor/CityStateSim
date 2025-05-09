using Gameplay.Services.Interfaces;
using Gameplay.Services;
using Gameplay.Simulation;
using Microsoft.Extensions.DependencyInjection;
using Gameplay.Simulation.Interfaces;
using Gameplay.Worlds;

namespace Gameplay.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameplaySimulation(this IServiceCollection services)
    {

        services.AddScoped<ISimulationRunner, SimulationRunner>();
        services.AddSingleton<ITickSpeedService, TickSpeedService>();

        services.AddScoped<IWorldTickSystem, MapGenerationSystem>();

        services.AddSingleton<MapGenerator>();
        return services;
    }
}
