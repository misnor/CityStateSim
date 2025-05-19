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
using CityStateSim.Gameplay.Worlds;
using CityStateSim.Core.Config;

namespace CityStateSim.UI.Rendering;
public class TileRenderSystem : IRenderSystem
{
    private Texture2D pixel;
    private readonly int tileSize = 32;
    private readonly ITextureFactory textureFactory;
    private readonly IEnumerable<TileDefinition> tileDefinitions;
    private readonly Dictionary<string, Texture2D> spriteCache = new();

    static readonly Dictionary<string, Color> TileColors = new()
    {
        ["grass"] = Color.Green,
        ["tree"] = new Color(0, 100, 0),
        ["rock"] = Color.DarkGray,
        ["stockpile"] = Color.Purple
    };

    // Inject your JSON-loaded definitions alongside the texture factory
    public TileRenderSystem(
        ITextureFactory textureFactory,
        List<TileDefinition> tileDefinitions)
    {
        this.textureFactory = textureFactory;
        this.tileDefinitions = tileDefinitions;
    }

    public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        pixel = textureFactory.GetTexture("whitePixel");

        foreach (var def in tileDefinitions)
        {
            if (!string.IsNullOrEmpty(def.SpriteKey))
            {
                spriteCache[def.Id] = textureFactory.GetTexture(def.SpriteKey);
            }
        }
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
            var rect = new Rectangle(
                pos.X * Constants.TileSize,
                pos.Y * Constants.TileSize,
                Constants.TileSize,
                Constants.TileSize);

            if (spriteCache.TryGetValue(type.Id, out var tex))
            {
                spriteBatch.Draw(tex, rect, Color.White);
            }
            else
            {
                var color = TileColors.TryGetValue(type.Id, out var c)
                            ? c
                            : Color.Magenta;   // guard against missing keys
                spriteBatch.Draw(pixel, rect, color);
            }

            if (HasJobAssigned(type.Id, e))
            {
                spriteBatch.Draw(pixel, rect, new Color(255, 0, 0, 100));
                int border = 2;
                spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, border), Color.Red);
                spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Bottom - border, rect.Width, border), Color.Red);
                spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, border, rect.Height), Color.Red);
                spriteBatch.Draw(pixel, new Rectangle(rect.Right - border, rect.Y, border, rect.Height), Color.Red);
            }
        }
    }

    private bool HasJobAssigned(string id, Entity entity)
    {
        if (!entity.Has<JobComponent>())
        {
            return false;
        }

        switch (id)
        {
            case "tree":
            case "rock":
                return true;
            default:
                return false;
        }
    }
}
