namespace CityStateSim.Infrastructure.Input;

/// <summary>
/// A platform-agnostic abstraction for reading player input.
/// </summary>
public interface IInputService
{
    bool IsKeyDown(InputKey key);

    bool WasKeyPressed(InputKey key);

    MousePosition GetMousePosition();

    bool IsMouseButtonDown(MouseButton button);

    bool WasMouseButtonClicked(MouseButton button);
}

public enum InputKey
{
    Escape,
    Space,
    Up,
    Down,
    Left,
    Right,
}

public enum MouseButton
{
    Left,
    Right,
    Middle
}

public readonly struct MousePosition
{
    public int X { get; }
    public int Y { get; }

    public MousePosition(int x, int y) => (X, Y) = (x, y);
}