using Core.Commands;
using Gameplay.Commands;
using Infrastructure.Application;

namespace Gameplay.Handlers;
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
