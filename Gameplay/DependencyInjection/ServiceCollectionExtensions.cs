using CityStateSim.Gameplay.Services.Interfaces;
using CityStateSim.Gameplay.Services;
using CityStateSim.Gameplay.Simulation;
using Microsoft.Extensions.DependencyInjection;
using CityStateSim.Gameplay.Simulation.Interfaces;
using CityStateSim.Gameplay.Worlds;
using CityStateSim.Gameplay.Systems;
using CityStateSim.Core.Commands;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Gameplay.Handlers;

namespace CityStateSim.Gameplay.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameplaySimulation(this IServiceCollection services)
    {

        services.AddScoped<ISimulationRunner, SimulationRunner>();
        services.AddScoped<IWorldTickSystem, MapGenerationSystem>();
        services.AddScoped<IWorldTickSystem, InputSystem>();
        services.AddSingleton<ITickSpeedService, TickSpeedService>();

        services.AddScoped<ICommandHandler<ExitGameCommand>, ExitGameCommandHandler>();
        services.AddScoped<ICommandHandler<TogglePauseCommand>, TogglePauseCommandHandler>();

        services.AddSingleton<MapGenerator>();
        return services;
    }
}
