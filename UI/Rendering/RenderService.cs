using System.Collections.Generic;
using DefaultEcs;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Rendering.Interfaces;

namespace UI.Rendering;
public class RenderService : IRenderService
{
    private readonly List<IRenderSystem> systems = new();

    public void Draw(SpriteBatch spriteBatch, World world)
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
        var pixel = contentManager.Load<Texture2D>("whitePixel");

        systems.Add(new TileRenderSystem(pixel));
    }
}
