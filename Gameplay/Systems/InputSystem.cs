using CityStateSim.Core.Commands;
using DefaultEcs;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Gameplay.Simulation.Interfaces;
using CityStateSim.Infrastructure.Input;

namespace CityStateSim.Gameplay.Systems;
public class InputSystem : IWorldTickSystem
{
    private readonly IInputService input;
    private readonly ICommandDispatcher dispatcher;
    private bool previousSpaceDown;

    public InputSystem(IInputService input, ICommandDispatcher dispatcher)
    {
        this.input = input;
        this.dispatcher = dispatcher;
        this.previousSpaceDown = false;
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
