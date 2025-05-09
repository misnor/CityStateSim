using Core.Components;
using DefaultEcs;
using Infrastructure.Config.Interfaces;
using Infrastructure.Factories.Interfaces;

namespace Gameplay.Worlds;

/// <summary>
/// Defines a single tile type as specified in tiles.json
/// </summary>
public record TileDefinition(string Id, bool Walkable, string? Resource, float SpawnProbability);

public class MapGenerator
{
    private readonly IConfigProvider config;
    private readonly IWorldFactory worldFactory;
    private readonly Random rand;

    public MapGenerator(IConfigProvider cfg, IWorldFactory wf, Random? rand = null)
    {
        this.config = cfg;
        this.worldFactory = wf;
        this.rand = rand ?? new Random();
    }

    /// <summary>
    /// Generates a width×height grid of tile entities.
    /// </summary>
    public World Generate(int width, int height)
    {
        var definitions = config.LoadConfig<List<TileDefinition>>("tiles.json");
        var world = worldFactory.CreateWorld();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var r = (float)rand.NextDouble();
                float cumulative = 0;
                TileDefinition choice = definitions[0];

                foreach (var def in definitions)
                {
                    cumulative += def.SpawnProbability;
                    if (r <= cumulative)
                    {
                        choice = def;
                        break;
                    }
                }

                var entity = world.CreateEntity();
                entity.Set(new PositionComponent(x, y));
                entity.Set(new TileTypeComponent(choice.Id));
            }
        }

        return world;
    }
}
