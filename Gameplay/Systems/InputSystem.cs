using Core.Commands;
using DefaultEcs;
using Gameplay.Commands;
using Gameplay.Simulation.Interfaces;
using Infrastructure.Input;

namespace Gameplay.Systems;
public class InputSystem : IWorldTickSystem
{
    private readonly IInputService input;
    private readonly ICommandDispatcher dispatcher;

    public InputSystem(IInputService input, ICommandDispatcher dispatcher)
    {
        this.input = input;
        this.dispatcher = dispatcher;
    }

    public void Update(World world)
    {
        if (input.IsKeyDown(InputKey.Escape))
        {
            dispatcher.Dispatch(new ExitGameCommand());
        }

        if (input.WasKeyPressed(InputKey.Space))
        {
            dispatcher.Dispatch(new TogglePauseCommand());
        }
    }
}
