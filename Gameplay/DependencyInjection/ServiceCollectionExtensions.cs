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
using CityStateSim.Gameplay.Jobs;
using CityStateSim.Core.Events;

namespace CityStateSim.Gameplay.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameplaySimulation(this IServiceCollection services)
    {
        services.AddScoped<ISimulationRunner, SimulationRunner>();
        services.AddScoped<IWorldTickSystem, MapGenerationSystem>();
        services.AddScoped<IWorldTickSystem, AgentMovementSystem>();
        services.AddScoped<IWorldTickSystem, JobAssignmentSystem>();
        services.AddScoped<IWorldTickSystem, InputSystem>();
        services.AddScoped<IWorldTickSystem, WorkStartSystem>();
        services.AddScoped<IWorldTickSystem, WorkProgressSystem>();

        services.AddSingleton<ITickSpeedService, TickSpeedService>();
        services.AddSingleton<IToolStateService, ToolStateService>();
        
        services.AddScoped<IJobHandler, CutTreeJobHandler>();
        services.AddScoped<IJobHandler, MineRockJobHandler>();
        services.AddScoped<IJobHandler, PickupResourceJobHandler>();
        services.AddScoped<IJobHandler, DeliverResourceJobHandler>();

        services.AddScoped<ICommandHandler<ExitGameCommand>, ExitGameCommandHandler>();
        services.AddScoped<ICommandHandler<TogglePauseCommand>, TogglePauseCommandHandler>();
        services.AddScoped<ICommandHandler<MarkTreesForCuttingCommand>, MarkTreesForCuttingCommandHandler>();
        services.AddScoped<ICommandHandler<MarkStonesForMiningCommand>, MarkRocksForMiningCommandHandler>();
        services.AddScoped<ICommandHandler<CancelJobCommand>, CancelJobCommandHandler>();
        services.AddScoped<ICommandHandler<CreatePickupJobCommand>, CreatePickupJobCommandHandler>();
        services.AddScoped<ICommandHandler<CreateDeliverJobCommand>, CreateDeliverJobCommandHandler>();

        services.AddSingleton<MapGenerator>();
        return services;
    }
}
