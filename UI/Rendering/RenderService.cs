using System.Collections.Generic;
using DefaultEcs;
using CityStateSim.Infrastructure.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.UI.Camera;
using CityStateSim.UI.Factories.Interfaces;
using CityStateSim.UI.Rendering.Interfaces;

namespace CityStateSim.UI.Rendering;
public class RenderService : IRenderService
{
    private readonly IEnumerable<IRenderSystem> systems;
    private readonly Camera2D camera;

    public RenderService(IEnumerable<IRenderSystem> systems,
            Camera2D camera)
    {
        this.systems = systems;
        this.camera = camera;
    }

    public void Draw(
        SpriteBatch spriteBatch, 
        World world)
    {
        // Draw world with camera transform
        spriteBatch.Begin(
            transformMatrix: camera.GetViewMatrix(spriteBatch.GraphicsDevice),
            samplerState: SamplerState.PointClamp);

        foreach (var sys in this.systems)
        {
            if (sys is not ToolbarRenderSystem && sys is not HoverRenderSystem)
            {
                sys.Draw(spriteBatch, world);
            }
        }
        spriteBatch.End();

        // Draw UI without camera transform
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var sys in this.systems)
        {
            if (sys is ToolbarRenderSystem || sys is HoverRenderSystem)
            {
                sys.Draw(spriteBatch, world);
            }
        }
        spriteBatch.End();
    }

    public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        foreach(var sys in systems)
        {
            sys.Initialize(graphicsDevice, contentManager);
        }
    }
}
