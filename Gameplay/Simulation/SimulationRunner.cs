using Core.EventBus.Interfaces;
using Core.Events;
using DefaultEcs;
using Gameplay.Simulation.Interfaces;

namespace Gameplay.Simulation;
public class SimulationRunner : ISimulationRunner
{
    private readonly IEcsEventBus ecsBus;
    private readonly World world;
    private readonly IEnumerable<IWorldTickSystem> systems;

    public SimulationRunner(IEcsEventBus ecsBus, World world, IEnumerable<IWorldTickSystem> systems)
    {
        this.ecsBus = ecsBus;
        this.world = world;
        this.systems = systems;
    }

    public void Tick()
    {
        this.ecsBus.Publish(new TickOccurred());

        foreach (var system in this.systems)
        {
            system.Update(this.world);
        }
    }
}
