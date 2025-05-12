using CityStateSim.Core.Commands;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Infrastructure.Application;

namespace CityStateSim.Gameplay.Handlers;
public class ExitGameCommandHandler : ICommandHandler<ExitGameCommand>
{
    private readonly IGameControl gameControl;

    public ExitGameCommandHandler(IGameControl gameControl)
    {
        this.gameControl = gameControl;
    }

    public void Handle(ExitGameCommand command)
    {
        gameControl.Exit();
    }
}
