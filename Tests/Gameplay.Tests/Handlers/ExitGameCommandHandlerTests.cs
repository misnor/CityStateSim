using CityStateSim.Gameplay.Commands;
using CityStateSim.Gameplay.Handlers;
using CityStateSim.Infrastructure.Application;

namespace CityStateSim.Gameplay.Tests.Handlers;

[TestFixture]
public class ExitGameCommandHandlerTests
{
    private class FakeGameControl : IGameControl
    {
        public bool ExitCalled { get; private set; }
        public void Exit() => ExitCalled = true;
    }

    [Test]
    public void Handle_ExitGameCommand_CallsGameControlExitOnce()
    {
        // Arrange
        var fakeControl = new FakeGameControl();
        var handler = new ExitGameCommandHandler(fakeControl);

        // Act
        handler.Handle(new ExitGameCommand());

        // Assert
        Assert.That(fakeControl.ExitCalled, Is.True,
            "ExitGameCommandHandler should call IGameControl.Exit().");
    }
}
