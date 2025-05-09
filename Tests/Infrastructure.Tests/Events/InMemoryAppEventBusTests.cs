using Infrastructure.Events;

namespace Infrastructure.Tests.Events;
[TestFixture]
public class InMemoryAppEventBusTests
{
    private InMemoryAppEventBus bus;

    [SetUp]
    public void SetUp()
    {
        bus = new InMemoryAppEventBus();
    }

    [Test]
    public void Subscribe_Publish_StringHandler_IsCalled()
    {
        // Arrange
        string received = null!;
        bus.Subscribe<string>(msg => received = msg);

        // Act
        bus.Publish("test");

        // Assert
        Assert.That(received, Is.EqualTo("test"));
    }

    [Test]
    public void Subscribe_Unsubscribe_StringHandler_IsNotCalledAfterDispose()
    {
        // Arrange
        int calls = 0;
        var sub = bus.Subscribe<string>(_ => calls++);
        sub.Dispose();

        // Act
        bus.Publish("ping");

        // Assert
        Assert.That(calls, Is.EqualTo(0));
    }

    [Test]
    public void Publish_MultipleHandlers_AllAreCalled()
    {
        // Arrange
        int countA = 0, countB = 0;
        bus.Subscribe<int>(_ => countA++);
        bus.Subscribe<int>(_ => countB++);

        // Act
        bus.Publish(42);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(countA, Is.EqualTo(1));
            Assert.That(countB, Is.EqualTo(1));
        });
    }

    [Test]
    public void Publish_NoSubscribers_DoesNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => bus.Publish(123));
    }

    [Test]
    public void HandlersOfDifferentTypes_DoNotInterfere()
    {
        // Arrange
        bool stringCalled = false;
        bool intCalled = false;

        bus.Subscribe<string>(_ => stringCalled = true);
        bus.Subscribe<int>(_ => intCalled = true);

        // Act
        bus.Publish("hello");

        // Assert
        Assert.That(stringCalled, Is.True);
        Assert.That(intCalled, Is.False);

        // Act again
        stringCalled = false;
        bus.Publish(99);

        // Assert
        Assert.That(stringCalled, Is.False);
        Assert.That(intCalled, Is.True);
    }
}
