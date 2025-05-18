using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
using CityStateSim.Gameplay.Commands;
using DefaultEcs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityStateSim.Gameplay.Tests.Commands;

[TestFixture]
public class CancelJobCommandHandlerTests
{
    private World world;
    private CancelJobCommandHandler handler;
    private ILogger<CancelJobCommandHandler> logger;

    [SetUp]
    public void SetUp()
    {
        world = new World();
        logger = NullLogger<CancelJobCommandHandler>.Instance;
        handler = new CancelJobCommandHandler(world, logger);
    }

    [TearDown]
    public void TearDown()
    {
        world.Dispose();
    }

    [Test]
    public void Handle_NoEntitiesInArea_DoesNothing()
    {
        // Arrange
        var command = new CancelJobCommand(0, 0, 5, 5);

        // Act
        handler.Handle(command);

        // Assert
        Assert.That(world.GetEntities().AsEnumerable().Count(), Is.EqualTo(0));
    }

    [Test]
    public void Handle_EntityWithJobInArea_RemovesJobTag()
    {
        // Arrange
        var entity = world.CreateEntity();
        entity.Set(new PositionComponent(2, 2));
        entity.Set<CutTreeJobTag>();

        var command = new CancelJobCommand(0, 0, 5, 5);

        // Act
        handler.Handle(command);

        // Assert
        Assert.That(entity.Has<JobComponent>(), Is.False);
    }

    [Test]
    public void Handle_EntityWithJobOutsideArea_KeepsJobTag()
    {
        // Arrange
        var entity = world.CreateEntity();
        entity.Set(new PositionComponent(10, 10));
        entity.Set<CutTreeJobTag>();

        var command = new CancelJobCommand(0, 0, 5, 5);

        // Act
        handler.Handle(command);

        // Assert
        Assert.That(entity.Has<CutTreeJobTag>(), Is.True);
    }

    [Test]
    public void Handle_MultipleEntitiesInArea_RemovesJobTagsFromAll()
    {
        // Arrange
        var entity1 = world.CreateEntity();
        entity1.Set(new PositionComponent(1, 1));
        entity1.Set<JobComponent>();

        var entity2 = world.CreateEntity();
        entity2.Set(new PositionComponent(2, 2));
        entity2.Set<JobComponent>();

        var entity3 = world.CreateEntity();
        entity3.Set(new PositionComponent(10, 10)); // Outside area
        entity3.Set<JobComponent>();

        var command = new CancelJobCommand(0, 0, 5, 5);

        // Act
        handler.Handle(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity1.Has<JobComponent>(), Is.False);
            Assert.That(entity2.Has<JobComponent>(), Is.False);
            Assert.That(entity3.Has<JobComponent>(), Is.True);
        });
    }

    [Test]
    public void Handle_EntityWithoutJobTag_DoesNotThrow()
    {
        // Arrange
        var entity = world.CreateEntity();
        entity.Set(new PositionComponent(2, 2));

        var command = new CancelJobCommand(0, 0, 5, 5);

        // Act & Assert
        Assert.DoesNotThrow(() => 
        {
            handler.Handle(command);
        });
    }
} 