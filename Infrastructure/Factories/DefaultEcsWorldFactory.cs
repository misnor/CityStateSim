using Infrastructure.Factories.Interfaces;

namespace Infrastructure.Factories;
public class DefaultEcsWorldFactory : IWorldFactory
{
    public DefaultEcs.World CreateWorld() => new DefaultEcs.World();
}
