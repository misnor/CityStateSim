// File: UI/Camera/Camera2D.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Extensions.Logging;

namespace CityStateSim.UI.Camera
{
    /// <summary>
    /// Simple 2D camera for panning, zooming, and converting screen ↔ world coordinates.
    /// </summary>
    public class Camera2D
    {
        private readonly ILogger<Camera2D> logger;

        /// <summary>Center of the camera in world space.</summary>
        public Vector2 Position { get; set; } = Vector2.Zero;

        /// <summary>Zoom factor (1 = 100%).</summary>
        public float Zoom { get; set; } = 1f;

        /// <summary>Rotation in radians.</summary>
        public float Rotation { get; set; } = 0f;

        public Camera2D(ILogger<Camera2D> logger)
        {
            this.logger = logger;
        }

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
            var result = Vector2.Transform(screenPosition, inv);
            logger.LogDebug("ScreenToWorld: ({ScreenX}, {ScreenY}) -> ({WorldX}, {WorldY})",
                screenPosition.X, screenPosition.Y, result.X, result.Y);
            return result;
        }

        /// <summary>
        /// Converts world coordinates to screen coordinates.
        /// </summary>
        public Vector2 WorldToScreen(Vector2 worldPos, GraphicsDevice graphicsDevice, bool centerOrigin = true)
        {
            var result = Vector2.Transform(worldPos, GetViewMatrix(graphicsDevice, centerOrigin));
            logger.LogDebug("WorldToScreen: ({WorldX}, {WorldY}) -> ({ScreenX}, {ScreenY})",
                worldPos.X, worldPos.Y, result.X, result.Y);
            return result;
        }
    }
}
