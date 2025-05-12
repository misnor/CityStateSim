using CityStateSim.Core.Commands;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Gameplay.Simulation.Interfaces;

namespace CityStateSim.Gameplay.Handlers;
public class TogglePauseCommandHandler : ICommandHandler<TogglePauseCommand>
{
    private readonly ISimulationRunner runner;
    
    public TogglePauseCommandHandler(ISimulationRunner runner)
    {
        this.runner = runner;
    }
    
    public void Handle(TogglePauseCommand command)
    {
        this.runner.TogglePause();
    }
}
