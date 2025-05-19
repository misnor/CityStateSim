using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
using DefaultEcs;
using CityStateSim.Infrastructure.Config.Interfaces;
using CityStateSim.Core.Enums;
using CityStateSim.Core.Config;

namespace CityStateSim.Gameplay.Worlds;


public class MapGenerator
{
    private readonly IConfigProvider config;
    private readonly Random rand;

    public int AgentsSpawned { get; private set; } = 0;

    public MapGenerator(IConfigProvider cfg,Random? rand = null)
    {
        this.config = cfg;
        this.rand = rand ?? new Random();
    }

    /// <summary>
    /// Generates a width×height grid of tile entities.
    /// </summary>
    public World Generate(World world, int width, int height)
    {
        var definitions = config.LoadConfig<List<TileDefinition>>("tiles.json");

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

        SpawnAgents(world);

        return world;
    }

    private void SpawnAgents(World world)
    {
        SpawnAgent(world, 2, 2);
        SpawnAgent(world, 4, 3);
        SpawnAgent(world, 7, 2);
        
    }

    private void SpawnAgent(World world, int x, int y)
    {
        this.AgentsSpawned++;
        var newAgent = world.CreateEntity();
        newAgent.Set(new PositionComponent(x, y));
        newAgent.Set(new AgentTag());
        newAgent.Set(new AgentStateComponent(AgentState.Idle));
        var spriteKey = $"humanoid{(AgentsSpawned % 3) + 1}";
        newAgent.Set(new SpriteComponent { TextureKey = spriteKey });
    }
}
