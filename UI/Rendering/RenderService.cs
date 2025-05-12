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
    private Camera2D camera;

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
        spriteBatch.Begin();
        foreach (var sys in this.systems)
        {
            sys.Draw(spriteBatch, world);
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
