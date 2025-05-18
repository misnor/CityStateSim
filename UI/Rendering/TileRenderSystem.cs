using System;
using System.Collections.Generic;
using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.UI.Factories.Interfaces;
using CityStateSim.UI.Rendering.Interfaces;
using CityStateSim.UI;

namespace CityStateSim.UI.Rendering;
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
            var rect = new Rectangle(pos.X * Constants.TileSize, pos.Y * Constants.TileSize, Constants.TileSize, Constants.TileSize);

            // Draw base tile
            spriteBatch.Draw(pixel, rect, TileColors[type.Id]);

            // If it's a tree marked for cutting, draw an overlay
            if (HasJobAssigned(type.Id, e))
            {
                // Draw a semi-transparent red overlay
                spriteBatch.Draw(pixel, rect, new Color(255, 0, 0, 100));

                // Draw a red border
                int borderWidth = 2;
                spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, borderWidth), Color.Red); // Top
                spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Bottom - borderWidth, rect.Width, borderWidth), Color.Red); // Bottom
                spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, borderWidth, rect.Height), Color.Red); // Left
                spriteBatch.Draw(pixel, new Rectangle(rect.Right - borderWidth, rect.Y, borderWidth, rect.Height), Color.Red); // Right
            }
        }
    }

    public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        this.pixel = this.textureFactory.GetTexture("whitePixel");
    }

    private bool HasJobAssigned(string id, Entity entity)
    {
        if (!entity.Has<JobComponent>())
        { 
            return false;
        }

        switch (id)
        {
            case "tree": return true;
            case "rock": return true;
            default:
                return false;
        }
    }
}
