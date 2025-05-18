using System;
using CityStateSim.Core.Commands;
using CityStateSim.Gameplay.Commands;
using CityStateSim.UI.Camera;
using Microsoft.Xna.Framework;

namespace CityStateSim.UI.Handlers;
public class MoveCameraCommandHandler : ICommandHandler<MoveCameraCommand>
{
    private readonly Camera2D camera;
    private const float CameraSpeed = 500f; // Pixels per second

    public MoveCameraCommandHandler(Camera2D camera)
    {
        this.camera = camera ?? throw new ArgumentNullException(nameof(camera));
    }

    public void Handle(MoveCameraCommand command)
    {
        // Convert the command's direction into a normalized vector
        var direction = new Vector2(command.DX, command.DY);
        if (direction != Vector2.Zero)
        {
            direction.Normalize();
        }

        // Apply movement based on fixed speed
        camera.Position += direction * CameraSpeed * (1f/60f); // Assuming 60 FPS as base
    }
}
