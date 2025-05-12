using System.Reflection.Metadata;
using Core.Components;
using Core.Components.Tags;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Factories.Interfaces;
using UI.Rendering.Interfaces;

namespace UI.Rendering;
public class AgentRenderSystem : IRenderSystem
{
    private Texture2D pixel;
    private readonly int tileSize = 32;
    private readonly ITextureFactory textureFactory;

    public AgentRenderSystem(ITextureFactory textureFactory)
    {
        this.textureFactory = textureFactory;
    }

    public void Draw(SpriteBatch spriteBatch, World world)
    {
        foreach (var e in world.GetEntities()
                       .With<PositionComponent>()
                       .With<AgentTag>()
                       .AsEnumerable())
        {
            ref var pos = ref e.Get<PositionComponent>();
            var rect = new Rectangle(pos.X * tileSize, pos.Y * tileSize, tileSize, tileSize);
            spriteBatch.Draw(pixel, rect, Color.Red);
        }
    }

    public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        this.pixel = this.textureFactory.GetTexture("whitePixel");
    }
}
