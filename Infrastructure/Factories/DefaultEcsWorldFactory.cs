using CityStateSim.Infrastructure.Factories.Interfaces;

namespace CityStateSim.Infrastructure.Factories;
public class DefaultEcsWorldFactory : IWorldFactory
{
    public DefaultEcs.World CreateWorld() => new DefaultEcs.World();
}
