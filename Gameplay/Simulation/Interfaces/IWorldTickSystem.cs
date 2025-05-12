using DefaultEcs;

namespace CityStateSim.Gameplay.Simulation.Interfaces;
public interface IWorldTickSystem
{
    void Update(World world);
}
