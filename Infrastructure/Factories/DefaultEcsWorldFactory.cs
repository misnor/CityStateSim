using System.Reflection.Metadata.Ecma335;
using DefaultEcs;
using Infrastructure.Factories.Interfaces;

namespace Infrastructure.Factories;
public class DefaultEcsWorldFactory : IWorldFactory
{
    public World CreateWorld() => new World();
}
