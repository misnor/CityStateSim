using Core.Components;
using Gameplay.Worlds;
using Infrastructure.Config.Interfaces;
using Infrastructure.Factories;
using Gameplay.Simulation;
using Core.Components.Tags;

namespace Gameplay.Tests.Worlds
{
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
            Assert.That(world.GetEntities().AsEnumerable().Count(), Is.EqualTo((200 * 200)+gen.AgentsSpawned));
            foreach (var e in world.GetEntities().AsEnumerable())
            {
                if(e.Has<AgentTag>())
                {
                    // Ignore agents for the purposes of this test
                    continue;
                }
                Assert.That(e.Get<TileTypeComponent>().Id, Is.EqualTo("only"));
            }
        }
    }

    [TestFixture]
    public class MapGenerationSystemTests
    {
        [Test]
        public void Update_CalledTwice_OnlyGeneratesOnce()
        {
            // Arrange
            var defs = new List<TileDefinition>
            {
                new("only", true, null, 1.0f)
            };
            var cfg = new StubConfigProvider(defs);
            var worldFactory = new DefaultEcsWorldFactory();
            var rand = new Random(42);
            var mapGenerator = new MapGenerator(cfg, rand);
            var system = new MapGenerationSystem(mapGenerator);
            var world = worldFactory.CreateWorld();

            // Act
            system.Update(world);
            var countAfterFirst = world.GetEntities().AsEnumerable().Count();
            system.Update(world);
            var countAfterSecond = world.GetEntities().AsEnumerable().Count();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(countAfterFirst, Is.EqualTo((200 * 200) + mapGenerator.AgentsSpawned));
                Assert.That(countAfterSecond, Is.EqualTo(countAfterFirst));
            });
        }
    }
}
