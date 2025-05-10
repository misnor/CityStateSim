using Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Gameplay.Simulation.Interfaces;
using Gameplay.DependencyInjection;

namespace IntegrationTests;
[TestFixture]
public class SimulationIntegrationTests
{
    private ServiceProvider _provider;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var services = new ServiceCollection()
          .AddInfrastructure()
          .AddConfiguration()
          .AddWorldFactory()
          .AddLogging(builder => builder.AddConsole())
          .AddEventBuses()
          .AddGameplaySimulation();

        _provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _provider.Dispose();

    [Test]
    public void FirstTick_PopulatesWorldWith200x200Tiles()
    {
        using var scope = _provider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<ISimulationRunner>();
        runner.Tick();

        var world = scope.ServiceProvider.GetRequiredService<DefaultEcs.World>();
        var count = world.GetEntities().AsEnumerable().Count();
        Assert.That(count, Is.EqualTo(200 * 200));
    }
}
