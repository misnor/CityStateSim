using DefaultEcs;

namespace Gameplay.Simulation.Interfaces;
public interface IWorldTickSystem
{
    void Update(World world);
}
