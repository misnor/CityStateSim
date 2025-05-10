using Core.Components;
using Gameplay.Worlds;
using Infrastructure.Config.Interfaces;
using Infrastructure.Factories;

namespace Gameplay.Tests.Worlds;

internal class StubConfigProvider : IConfigProvider
{
    private readonly object _payload;
    public StubConfigProvider(object payload) => _payload = payload;
    public T LoadConfig<T>(string fileName) => (T)_payload!;
}

[TestFixture]
public class MapGeneratorTests
{
    [Test]
    public void MapGenerator_SingleType_AllEntitiesMatch()
    {
        // Arrange
        var defs = new List<TileDefinition> 
        {
            new("only", true, null, 1.0f)
        };
        var cfg = new StubConfigProvider(defs);
        var worldFactory = new DefaultEcsWorldFactory();
        var rand = new Random(42);
        var gen = new MapGenerator(cfg, rand);

        // Act
        var world = gen.Generate(worldFactory.CreateWorld(), 200, 200);

        // Assert
        Assert.That(world.GetEntities().AsEnumerable().Count(), Is.EqualTo(200*200));
        foreach (var e in world.GetEntities().AsEnumerable())
        {
            Assert.That(e.Get<TileTypeComponent>().Id, Is.EqualTo("only"));
        }
    }
}
