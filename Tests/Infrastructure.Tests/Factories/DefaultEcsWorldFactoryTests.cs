using DefaultEcs;
using CityStateSim.Infrastructure.Factories;

namespace CityStateSim.Infrastructure.Tests.Factories;

[TestFixture]
public class DefaultEcsWorldFactoryTests
{
    private DefaultEcsWorldFactory factory;

    [SetUp]
    public void SetUp()
    {
        factory = new DefaultEcsWorldFactory();
    }

    [Test]
    public void CreateWorld_ReturnsNonNullWorld()
    {
        // Act
        World world = factory.CreateWorld();

        // Assert
        Assert.NotNull(world);
        Assert.IsInstanceOf<World>(world);
    }

    [Test]
    public void CreateWorld_ReturnsDistinctInstances()
    {
        // Act
        World first = factory.CreateWorld();
        World second = factory.CreateWorld();

        // Assert
        Assert.That(second, Is.Not.SameAs(first));
    }
}
