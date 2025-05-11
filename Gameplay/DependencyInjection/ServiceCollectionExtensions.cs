using Gameplay.Services.Interfaces;
using Gameplay.Services;
using Gameplay.Simulation;
using Microsoft.Extensions.DependencyInjection;
using Gameplay.Simulation.Interfaces;
using Gameplay.Worlds;
using Gameplay.Systems;
using Core.Commands;
using Gameplay.Commands;
using Gameplay.Handlers;

namespace Gameplay.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameplaySimulation(this IServiceCollection services)
    {

        services.AddScoped<ISimulationRunner, SimulationRunner>();
        services.AddScoped<IWorldTickSystem, MapGenerationSystem>();
        services.AddScoped<IWorldTickSystem, InputSystem>();
        services.AddSingleton<ITickSpeedService, TickSpeedService>();

        services.AddScoped<ICommandHandler<ExitGameCommand>, ExitGameCommandHandler>();

        services.AddSingleton<MapGenerator>();
        return services;
    }
}
