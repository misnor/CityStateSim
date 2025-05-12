using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Camera;
public class Camera2D
{
    /// <summary>
    /// Center-point of the camera in world space.
    /// </summary>
    public Vector2 Position { get; set; } = Vector2.Zero;

    /// <summary>
    /// Zoom factor (1 = 100%).
    /// </summary>
    public float Zoom { get; set; } = 1f;

    /// <summary>
    /// Rotation in radians.
    /// </summary>
    public float Rotation { get; set; } = 0f;

    /// <summary>
    /// Builds the view matrix for SpriteBatch.Begin.
    /// </summary>
    public Matrix GetViewMatrix(GraphicsDevice graphicsDevice)
    {
        Vector2 origin = new Vector2(
            graphicsDevice.Viewport.Width / 2f,
            graphicsDevice.Viewport.Height / 2f);

        return
            Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1f) *
            Matrix.CreateTranslation(new Vector3(origin, 0f));
    }

    /// <summary>
    /// Converts a screen-space point to world-space, accounting for Position, Zoom, Rotation.
    /// </summary>
    public Vector2 ScreenToWorld(Vector2 screenPosition, GraphicsDevice graphicsDevice)
    {
        Matrix inv = Matrix.Invert(GetViewMatrix(graphicsDevice));
        return Vector2.Transform(screenPosition, inv);
    }
}
