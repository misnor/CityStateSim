using System;
using System.Collections.Generic;
using Core.Components;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Factories.Interfaces;
using UI.Rendering.Interfaces;

namespace UI.Rendering;
public class TileRenderSystem : IRenderSystem
{
    private Texture2D pixel;
    private readonly int tileSize = 32;
    private readonly ITextureFactory textureFactory;

    static readonly Dictionary<string, Color> TileColors = new()
    {
        ["grass"] = Color.Green,
        ["tree"] = new Color(0, 100, 0),
        ["rock"] = Color.DarkGray,
        ["stockpile"] = Color.Purple
    };

    public TileRenderSystem(ITextureFactory textureFactory)
    {
        this.textureFactory = textureFactory;
    }

    public void Draw(SpriteBatch spriteBatch, World world)
    {
        foreach (var e in world.GetEntities()
                       .With<PositionComponent>()
                       .With<TileTypeComponent>()
                       .AsEnumerable())
        {
            ref var pos = ref e.Get<PositionComponent>();
            ref var type = ref e.Get<TileTypeComponent>();
            var rect = new Rectangle(pos.X * tileSize, pos.Y * tileSize, tileSize, tileSize);
            spriteBatch.Draw(pixel, rect, TileColors[type.Id]);
        }
    }

    public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        this.pixel = this.textureFactory.GetTexture("whitePixel");
    }
}
