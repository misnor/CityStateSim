using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Gameplay.Handlers;
using DefaultEcs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityStateSim.Gameplay.Tests.Commands;

[TestFixture]
public class MarkTreesForCuttingCommandHandlerTests
{
    private World world;
    private MarkTreesForCuttingCommandHandler handler;

    [SetUp]
    public void SetUp()
    {
        world = new World();
        handler = new MarkTreesForCuttingCommandHandler(world);
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
        var command = new MarkTreesForCuttingCommand(0, 0, 5, 5);

        // Act
        handler.Handle(command);

        // Assert
        Assert.That(world.GetEntities().AsEnumerable().Count(), Is.EqualTo(0));
    }

    [Test]
    public void Handle_TreeInArea_AddsCutTreeJobTag()
    {
        // Arrange
        var entity = world.CreateEntity();
        entity.Set(new PositionComponent(2, 2));
        entity.Set(new TileTypeComponent { Id = "tree" });

        var command = new MarkTreesForCuttingCommand(0, 0, 5, 5);

        // Act
        handler.Handle(command);

        // Assert
        Assert.That(entity.Has<CutTreeJobTag>(), Is.True);
    }

    [Test]
    public void Handle_TreeOutsideArea_DoesNotAddCutTreeJobTag()
    {
        // Arrange
        var entity = world.CreateEntity();
        entity.Set(new PositionComponent(10, 10));
        entity.Set(new TileTypeComponent { Id = "tree" });

        var command = new MarkTreesForCuttingCommand(0, 0, 5, 5);

        // Act
        handler.Handle(command);

        // Assert
        Assert.That(entity.Has<CutTreeJobTag>(), Is.False);
    }

    [Test]
    public void Handle_NonTreeEntityInArea_DoesNotAddCutTreeJobTag()
    {
        // Arrange
        var entity = world.CreateEntity();
        entity.Set(new PositionComponent(2, 2));
        entity.Set(new TileTypeComponent { Id = "grass" });

        var command = new MarkTreesForCuttingCommand(0, 0, 5, 5);

        // Act
        handler.Handle(command);

        // Assert
        Assert.That(entity.Has<CutTreeJobTag>(), Is.False);
    }

    [Test]
    public void Handle_MultipleTreesInArea_AddsCutTreeJobTagToAll()
    {
        // Arrange
        var entity1 = world.CreateEntity();
        entity1.Set(new PositionComponent(1, 1));
        entity1.Set(new TileTypeComponent { Id = "tree" });

        var entity2 = world.CreateEntity();
        entity2.Set(new PositionComponent(2, 2));
        entity2.Set(new TileTypeComponent { Id = "tree" });

        var entity3 = world.CreateEntity();
        entity3.Set(new PositionComponent(10, 10)); // Outside area
        entity3.Set(new TileTypeComponent { Id = "tree" });

        var command = new MarkTreesForCuttingCommand(0, 0, 5, 5);

        // Act
        handler.Handle(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity1.Has<CutTreeJobTag>(), Is.True);
            Assert.That(entity2.Has<CutTreeJobTag>(), Is.True);
            Assert.That(entity3.Has<CutTreeJobTag>(), Is.False);
        });
    }

    [Test]
    public void Handle_TreeAlreadyHasCutTreeJobTag_DoesNotThrow()
    {
        // Arrange
        var entity = world.CreateEntity();
        entity.Set(new PositionComponent(2, 2));
        entity.Set(new TileTypeComponent { Id = "tree" });
        entity.Set<CutTreeJobTag>();

        var command = new MarkTreesForCuttingCommand(0, 0, 5, 5);

        // Act & Assert
        Assert.DoesNotThrow(() => 
        {
            handler.Handle(command);
        });
    }
} 