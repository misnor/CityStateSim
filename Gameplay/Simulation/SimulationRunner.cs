using Core.EventBus.Interfaces;
using Core.Events;
using DefaultEcs;
using Gameplay.Simulation.Interfaces;
using Microsoft.Extensions.Logging;

namespace Gameplay.Simulation;
public class SimulationRunner : ISimulationRunner
{
    private readonly ILogger<SimulationRunner> logger;
    private readonly IEcsEventBus ecsBus;
    private readonly World world;
    private readonly IEnumerable<IWorldTickSystem> systems;
    private bool isPaused = false;

    public SimulationRunner(ILogger<SimulationRunner> logger, IEcsEventBus ecsBus, World world, IEnumerable<IWorldTickSystem> systems)
    {
        this.logger = logger;
        this.ecsBus = ecsBus;
        this.world = world;
        this.systems = systems;
    }

    public bool IsPaused => this.isPaused;

    public void Tick()
    {
        this.ecsBus.Publish(new TickOccurred());
        this.logger.LogInformation("Tick occurred.");
        foreach (var system in this.systems)
        {
            system.Update(this.world);
        }
    }

    public void TogglePause()
    {
        this.isPaused = !this.isPaused;
    }
}
