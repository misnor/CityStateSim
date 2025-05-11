using Core.EventBus.Interfaces;
using DefaultEcs;
using Gameplay.Simulation.Interfaces;
using Gameplay.Simulation;
using Microsoft.Extensions.Logging.Abstractions;

namespace Gameplay.Tests.Simulation;

[TestFixture]
public class SimulationRunnerTests
{
    private class SpySystem : IWorldTickSystem
    {
        public int UpdateCount { get; private set; }

        public void Update(World world) => UpdateCount++;
    }

    private class SpyEventBus : IEcsEventBus
    {
        public int PublishCount { get; private set; }
        public void Publish<T>(T evt) => PublishCount++;

        public IDisposable Subscribe<T>(Action<T> handler)
        {
            throw new NotImplementedException();
        }
    }

    private World world = null!;
    private SpyEventBus eventBus = null!;
    private SpySystem systemA = null!;
    private SpySystem systemB = null!;
    private SimulationRunner runner = null!;

    [SetUp]
    public void SetUp()
    {
        // set up a real world (InputSystem not included so no input gets invoked)
        world = new World();
        eventBus = new SpyEventBus();
        systemA = new SpySystem();
        systemB = new SpySystem();

        // pass only our two spy systems
        var systems = new List<IWorldTickSystem> { systemA, systemB };

        runner = new SimulationRunner(
            NullLogger<SimulationRunner>.Instance,
            eventBus,
            world,
            systems);
    }

    [TearDown]
    public void TearDown()
    {
        world.Dispose();
    }

    [Test]
    public void Tick_WhenUnpaused_PublishesTickAndRunsAllSystems()
    {
        Assert.That(runner.IsPaused, Is.False);

        runner.Tick();

        Assert.That(eventBus.PublishCount, Is.EqualTo(1),
            "Should publish exactly one TickOccurred event when unpaused");
        Assert.That(systemA.UpdateCount, Is.EqualTo(1),
            "All non-input systems should run exactly once when unpaused");
        Assert.That(systemB.UpdateCount, Is.EqualTo(1));
    }

    [Test]
    public void TogglePause_FlipsIsPausedFlag()
    {
        Assert.That(runner.IsPaused, Is.False);

        runner.TogglePause();
        Assert.That(runner.IsPaused, Is.True,
            "TogglePause should set IsPaused = true when starting from false");

        runner.TogglePause();
        Assert.That(runner.IsPaused, Is.False,
            "TogglePause should set IsPaused = false when starting from true");
    }

    [Test]
    public void Tick_WhenPaused_DoesNotPublishOrRunOtherSystems()
    {
        runner.TogglePause();
        Assert.That(runner.IsPaused, Is.True);

        runner.Tick();

        Assert.That(eventBus.PublishCount, Is.Zero,
            "Should not publish any TickOccurred events when paused");
        Assert.That(systemA.UpdateCount, Is.Zero,
            "Should not run non-input systems when paused");
        Assert.That(systemB.UpdateCount, Is.Zero);
    }
}
