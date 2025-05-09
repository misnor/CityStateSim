using DefaultEcs;
using Infrastructure.Events;

namespace Infrastructure.Tests.Events;

[TestFixture]
public class EcsEventBusAdapterTests
{
    private World world;
    private EcsEventBusAdapter adapter;

    [SetUp]
    public void SetUp()
    {
        world = new World();
        adapter = new EcsEventBusAdapter(world);
    }

    [TearDown]
    public void TearDown()
    {
        world.Dispose();
    }

    [Test]
    public void Subscribe_Publish_StringHandler_IsCalled()
    {
        // Arrange
        string received = null!;
        adapter.Subscribe<string>(msg => received = msg);

        // Act
        adapter.Publish("hello");

        // Assert
        Assert.That(received, Is.EqualTo("hello"));
    }

    [Test]
    public void Subscribe_Unsubscribe_StringHandler_IsNotCalledAfterDispose()
    {
        // Arrange
        int callCount = 0;
        var sub = adapter.Subscribe<string>(_ => callCount++);
        sub.Dispose();

        // Act
        adapter.Publish("ping");

        // Assert
        Assert.That(callCount, Is.EqualTo(0));
    }

    [Test]
    public void Publish_MultipleHandlers_AllAreCalled()
    {
        // Arrange
        int countA = 0, countB = 0;
        adapter.Subscribe<int>(_ => countA++);
        adapter.Subscribe<int>(_ => countB++);

        // Act
        adapter.Publish(42);

        // Assert
        Assert.That(countA, Is.EqualTo(1));
        Assert.That(countB, Is.EqualTo(1));
    }

    [Test]
    public void Publish_NoSubscribers_DoesNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => adapter.Publish(123));
    }
}