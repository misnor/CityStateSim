// File: UI/Camera/Camera2D.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Camera
{
    /// <summary>
    /// Simple 2D camera for panning, zooming, and converting screen ↔ world coordinates.
    /// </summary>
    public class Camera2D
    {
        /// <summary>Center of the camera in world space.</summary>
        public Vector2 Position { get; set; } = Vector2.Zero;

        /// <summary>Zoom factor (1 = 100%).</summary>
        public float Zoom { get; set; } = 1f;

        /// <summary>Rotation in radians.</summary>
        public float Rotation { get; set; } = 0f;

        /// <summary>
        /// Builds the view matrix.
        /// If centerOrigin is true, screen (0,0) maps to world (–Position) at screen center.
        /// If false, screen (0,0) maps directly to world (–Position) at top-left.
        /// </summary>
        public Matrix GetViewMatrix(GraphicsDevice graphicsDevice, bool centerOrigin = true)
        {
            Vector2 origin = centerOrigin
                ? new Vector2(
                    graphicsDevice.Viewport.Width / 2f,
                    graphicsDevice.Viewport.Height / 2f)
                : Vector2.Zero;

            return
                Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1f) *
                Matrix.CreateTranslation(new Vector3(origin, 0f));
        }

        /// <summary>
        /// Converts a screen-space point to world-space, using the same centering logic.
        /// </summary>
        public Vector2 ScreenToWorld(
            Vector2 screenPosition,
            GraphicsDevice graphicsDevice,
            bool centerOrigin = true)
        {
            Matrix view = GetViewMatrix(graphicsDevice, centerOrigin);
            Matrix inv = Matrix.Invert(view);
            return Vector2.Transform(screenPosition, inv);
        }
    }
}
