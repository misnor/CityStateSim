using CityStateSim.Gameplay.Commands;
using CityStateSim.Gameplay.Handlers;
using CityStateSim.Gameplay.Simulation.Interfaces;

namespace CityStateSim.Gameplay.Tests.Handlers;

[TestFixture]
public class TogglePauseCommandHandlerTests
{
    private class FakeSimulationRunner : ISimulationRunner
    {
        public bool TogglePauseCalled { get; private set; }
        public bool IsPaused => false;
        public void Tick() { }
        public void TogglePause() => TogglePauseCalled = true;
    }

    [Test]
    public void Handle_TogglePauseCommand_CallsTogglePauseOnRunner()
    {
        // Arrange
        var fakeRunner = new FakeSimulationRunner();
        var handler = new TogglePauseCommandHandler(fakeRunner);

        // Act
        handler.Handle(new TogglePauseCommand());

        // Assert
        Assert.That(fakeRunner.TogglePauseCalled, Is.True,
            "TogglePauseCommandHandler should call ISimulationRunner.TogglePause().");
    }
}
