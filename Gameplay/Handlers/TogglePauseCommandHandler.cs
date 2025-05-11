using Core.Commands;
using Gameplay.Commands;
using Gameplay.Simulation.Interfaces;

namespace Gameplay.Handlers;
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
