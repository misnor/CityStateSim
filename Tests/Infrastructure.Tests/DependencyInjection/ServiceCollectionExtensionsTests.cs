using Core.EventBus.Interfaces;
using DefaultEcs;
using Infrastructure.DependencyInjection;
using Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tests.DependencyInjection;
[TestFixture]
public class ServiceCollectionExtensionsTests
{
    private ServiceProvider provider;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var services = new ServiceCollection()
            .AddWorldFactory()
            .AddEventBuses();

        provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        provider.Dispose();
    }

    [Test]
    public void BuildServiceProvider_DoesNotThrow()
    {
        Assert.Pass("Container built successfully under ValidateScopes.");
    }

    [Test]
    public void AppEventBus_IsSingleton()
    {
        var bus1 = provider.GetRequiredService<IAppEventBus>();
        var bus2 = provider.GetRequiredService<IAppEventBus>();
        Assert.That(bus1, Is.SameAs(bus2));
        Assert.That(bus1, Is.InstanceOf<InMemoryAppEventBus>());
    }

    [Test]
    public void World_IsScoped()
    {
        // In scope A
        using (var scopeA = provider.CreateScope())
        {
            var w1 = scopeA.ServiceProvider.GetRequiredService<World>();
            var w2 = scopeA.ServiceProvider.GetRequiredService<World>();
            Assert.That(w1, Is.SameAs(w2), "Within the same scope, World should be identical");
        }
        // In scope B
        using (var scopeB = provider.CreateScope())
        {
            var w3 = scopeB.ServiceProvider.GetRequiredService<World>();
            Assert.That(w3, Is.Not.Null);

            // World from a new scope must differ from scope A’s world
            using (var scopeA = provider.CreateScope())
            {
                var w1 = scopeA.ServiceProvider.GetRequiredService<World>();
                Assert.That(w1, Is.Not.SameAs(w3), "Different scopes should each get a new World");
            }
        }
    }

    [Test]
    public void EcsEventBus_IsScopedAndBacksSameWorld()
    {
        using (var scope = provider.CreateScope())
        {
            var world = scope.ServiceProvider.GetRequiredService<World>();
            var bus = scope.ServiceProvider.GetRequiredService<IEcsEventBus>();

            // bus must be the adapter type
            Assert.That(bus, Is.InstanceOf<EcsEventBusAdapter>());

            // reflect into the adapter to grab its private _world field
            var field = typeof(EcsEventBusAdapter)
                .GetField("world", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var busWorld = (World)field!.GetValue(bus)!;

            Assert.That(busWorld, Is.SameAs(world),
                "EcsEventBusAdapter should be constructed with the same scoped World");
        }
    }

    [Test]
    public void EcsEventBus_SubscribePublish_Works()
    {
        using (var scope = provider.CreateScope())
        {
            var bus = scope.ServiceProvider.GetRequiredService<IEcsEventBus>();
            string received = null!;
            bus.Subscribe<string>(msg => received = msg);
            bus.Publish("ping");
            Assert.That(received, Is.EqualTo("ping"));
        }
    }
}