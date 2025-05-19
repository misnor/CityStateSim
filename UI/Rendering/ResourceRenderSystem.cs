using System.Collections.Generic;
using System.Linq;
using CityStateSim.Core.Components;
using CityStateSim.Core.Config;
using CityStateSim.UI.Factories.Interfaces;
using CityStateSim.UI.Rendering.Interfaces;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CityStateSim.UI.Rendering;
public class ResourceRenderSystem : IRenderSystem
{
    private readonly ITextureFactory textureFactory;
    private readonly List<ResourceDefinition> resources;
    private readonly Dictionary<string, Texture2D> cache = new();
    
    public ResourceRenderSystem(ITextureFactory textureFactory, List<ResourceDefinition> resources)
    {
        this.textureFactory = textureFactory;
        this.resources = resources;
    }
    public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        foreach (var resource in resources)
        {
            if (string.IsNullOrWhiteSpace(resource.SpriteKey))
            {
                continue;
            }

            cache[resource.Id] = textureFactory.GetTexture(resource.SpriteKey);
        }
    }

    public void Draw(SpriteBatch spriteBatch, World world)
    {
        foreach(var entity in world.GetEntities()
            .With<PositionComponent>()
            .With<ResourceComponent>()
            .AsEnumerable())
        {
            var pos = entity.Get<PositionComponent>();
            var resource = entity.Get<ResourceComponent>();
            var def = resources.FirstOrDefault(r => r.Id == resource.ResourceType);
            if (def == null)
            {
                continue; // no sprite found
            }

            var tex = cache[def.Id] ?? textureFactory.GetTexture("whitePixel");
            var x = pos.X * Constants.TileSize + def.OffsetX;
            var y = pos.Y * Constants.TileSize + def.OffsetY;
            var dest = new Rectangle(x, y, Constants.TileSize, Constants.TileSize);

            spriteBatch.Draw(tex, dest, Color.White);
        }
    }
}
