using System.Runtime.CompilerServices;
using DefaultEcs;
using CityStateSim.Gameplay.Simulation.Interfaces;
using CityStateSim.Gameplay.Worlds;

namespace CityStateSim.Gameplay.Simulation;
public class MapGenerationSystem : IWorldTickSystem
{
    private readonly MapGenerator mapGenerator;
    private bool generated = false;

    public MapGenerationSystem(MapGenerator mapGenerator)
    {
        this.mapGenerator = mapGenerator;
    }
    
    public void Update(World world)
    {
        if (generated)
        {
            return;
        }

        mapGenerator.Generate(world, 200, 200);
        generated = true;
    }
}
