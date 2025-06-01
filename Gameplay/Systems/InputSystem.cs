using CityStateSim.Core.Commands;
using DefaultEcs;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Gameplay.Simulation.Interfaces;
using CityStateSim.Infrastructure.Input;
using CityStateSim.Core.Events;
using CityStateSim.Core.EventBus.Interfaces;

namespace CityStateSim.Gameplay.Systems;
public class InputSystem : IWorldTickSystem
{
    private readonly IInputService input;
    private readonly ICommandDispatcher dispatcher;
    private readonly IAppEventBus bus;
    private bool previousSpaceDown;

    public InputSystem(IInputService input, ICommandDispatcher dispatcher, IAppEventBus bus)
    {
        this.input = input;
        this.dispatcher = dispatcher;
        this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
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

        // Fire SetSpeedCommand for 1/2/3 keys  
        if (input.WasKeyPressed(InputKey.D1))
        {
            bus.Publish(new SetSpeedCommand(1));
        }
        else if (input.WasKeyPressed(InputKey.D2))
        {
            bus.Publish(new SetSpeedCommand(2));
        }
        else if (input.WasKeyPressed(InputKey.D3))
        {
            bus.Publish(new SetSpeedCommand(5));
        }

    }
}
