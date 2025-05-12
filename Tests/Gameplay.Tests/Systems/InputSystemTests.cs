using CityStateSim.Core.Commands;
using DefaultEcs;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Gameplay.Systems;
using CityStateSim.Infrastructure.Input;

namespace CityStateSim.Gameplay.Tests.Systems;

[TestFixture]
public class InputSystemTests
{
    private class FakeInputService : IInputService
    {
        public bool EscapeDown { get; set; }
        public bool SpacePressed { get; set; }

        public bool IsKeyDown(InputKey key) =>
            key == InputKey.Escape && EscapeDown;

        public bool WasKeyPressed(InputKey key) =>
            key == InputKey.Space && SpacePressed;

        public bool IsMouseButtonDown(MouseButton button) => false;
        public bool WasMouseButtonClicked(MouseButton button) => false;
        public MousePosition GetMousePosition() => new MousePosition(0, 0);
    }

    private class SpyDispatcher : ICommandDispatcher
    {
        public readonly List<ICommand> Dispatched = new();

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            Dispatched.Add(command);
        }
    }

    private World dummyWorld;

    [SetUp]
    public void Setup()
    {
        dummyWorld = new World();
    }

    [TearDown]
    public void Teardown()
    {
        dummyWorld.Dispose();
    }

    [Test]
    public void Update_EscapeDown_DispatchesExitGameCommand()
    {
        var input = new FakeInputService { EscapeDown = true };
        var dispatcher = new SpyDispatcher();
        var system = new InputSystem(input, dispatcher);

        system.Update(dummyWorld);

        Assert.That(dispatcher.Dispatched, Has.Exactly(1)
            .InstanceOf<ExitGameCommand>());
    }

    [Test]
    public void Update_SpacePressed_DispatchesTogglePauseCommand()
    {
        var input = new FakeInputService { SpacePressed = true };
        var dispatcher = new SpyDispatcher();
        var system = new InputSystem(input, dispatcher);

        system.Update(dummyWorld);

        Assert.That(dispatcher.Dispatched, Has.Exactly(1)
            .InstanceOf<TogglePauseCommand>());
    }

    [Test]
    public void Update_BothKeys_DispatchesBothCommandsInOrder()
    {
        var input = new FakeInputService { EscapeDown = true, SpacePressed = true };
        var dispatcher = new SpyDispatcher();
        var system = new InputSystem(input, dispatcher);

        system.Update(dummyWorld);

        Assert.That(dispatcher.Dispatched.Count, Is.EqualTo(2));
        Assert.That(dispatcher.Dispatched[0], Is.TypeOf<ExitGameCommand>());
        Assert.That(dispatcher.Dispatched[1], Is.TypeOf<TogglePauseCommand>());
    }

    [Test]
    public void Update_NoKeys_DispatchesNothing()
    {
        var input = new FakeInputService(); // both flags default to false
        var dispatcher = new SpyDispatcher();
        var system = new InputSystem(input, dispatcher);

        system.Update(dummyWorld);

        Assert.That(dispatcher.Dispatched, Is.Empty);
    }
}
