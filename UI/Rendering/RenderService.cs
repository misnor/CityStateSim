using System.Collections.Generic;
using DefaultEcs;
using Infrastructure.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Camera;
using UI.Factories.Interfaces;
using UI.Rendering.Interfaces;

namespace UI.Rendering;
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
