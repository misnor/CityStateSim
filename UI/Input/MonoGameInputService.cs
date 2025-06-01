using System;
using CityStateSim.Infrastructure.Input;
using Microsoft.Xna.Framework.Input;

namespace CityStateSim.UI.Input;
public class MonoGameInputService : IInputService
{
    private KeyboardState previousKb;
    private MouseState previousMs;

    public bool IsKeyDown(InputKey key)
        => Keyboard.GetState().IsKeyDown(MapKey(key));

    public bool WasKeyPressed(InputKey key)
        => Keyboard.GetState().IsKeyDown(MapKey(key))
           && previousKb.IsKeyUp(MapKey(key));

    public MousePosition GetMousePosition()
    {
        var m = Mouse.GetState();
        return new MousePosition(m.X, m.Y);
    }

    public bool IsMouseButtonDown(MouseButton button)
        => GetButtonState(button) == ButtonState.Pressed;

    public bool WasMouseButtonClicked(MouseButton button)
        => GetButtonState(button) == ButtonState.Pressed
           && GetPreviousButtonState(button) == ButtonState.Released;

    private Keys MapKey(InputKey key) => key switch
    {
        InputKey.Escape => Keys.Escape,
        InputKey.Space => Keys.Space,
        InputKey.D1 => Keys.D1,
        InputKey.D2 => Keys.D2,
        InputKey.D3 => Keys.D3,
        _ => throw new ArgumentOutOfRangeException(nameof(key))
    };

    private ButtonState GetButtonState(MouseButton b) =>
        b switch
        {
            MouseButton.Left => Mouse.GetState().LeftButton,
            MouseButton.Right => Mouse.GetState().RightButton,
            MouseButton.Middle => Mouse.GetState().MiddleButton,
            _ => throw new ArgumentOutOfRangeException(nameof(b))
        };

    private ButtonState GetPreviousButtonState(MouseButton b) =>
        b switch
        {
            MouseButton.Left => previousMs.LeftButton,
            MouseButton.Right => previousMs.RightButton,
            MouseButton.Middle => previousMs.MiddleButton,
            _ => throw new ArgumentOutOfRangeException(nameof(b))
        };
}
