using System.Reflection.Metadata;
using Core.Components;
using Core.Components.Tags;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Rendering.Interfaces;

namespace UI.Rendering;
public class AgentRenderSystem : IRenderSystem
{
    private readonly Texture2D pixel;
    private readonly int tileSize;

    public AgentRenderSystem(Texture2D pixel, int tileSize = 32)
    {
        this.pixel = pixel;
        this.tileSize = tileSize;
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
}
