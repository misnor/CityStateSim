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

    /*    if (input.IsKeyDown(InputKey.Up))
        {
            dispatcher.Dispatch(new MoveCameraCommand(Direction.Up));
        }
        if (input.IsKeyDown(InputKey.Down))
        {
            dispatcher.Dispatch(new MoveCameraCommand(Direction.Down));
        }
        if (input.IsKeyDown(InputKey.Left))
        {
            dispatcher.Dispatch(new MoveCameraCommand(Direction.Left));
        }
        if (input.IsKeyDown(InputKey.Right))
        {
            dispatcher.Dispatch(new MoveCameraCommand(Direction.Right));
        }*/

        // Example: handle mouse clicks
       /* var mousePos = input.GetMousePosition();
        if (input.WasMouseButtonClicked(MouseButton.Left))
        {
            dispatcher.Dispatch(new LeftClickCommand(mousePos.X, mousePos.Y));
        }
        if (input.WasMouseButtonClicked(MouseButton.Right))
        {
            dispatcher.Dispatch(new RightClickCommand(mousePos.X, mousePos.Y));
        }*/
    }
}
