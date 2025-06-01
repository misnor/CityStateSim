using CityStateSim.Core.Events;
using CityStateSim.Gameplay.Services;
using CityStateSim.Infrastructure.Events;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityStateSim.Gameplay.Tests
{
    public class TickSpeedServiceTests
    {
        private InMemoryAppEventBus bus;
        private TickSpeedService service;

        [SetUp]
        public void Setup()
        {
            bus = new InMemoryAppEventBus();
            service = new TickSpeedService(bus, NullLogger<TickSpeedService>.Instance);
        }

        [Test]
        public void CurrentMultiplier_DefaultsTo1()
        {
            // Assert
            Assert.That(service.CurrentMultiplier, Is.EqualTo(1));
        }

        [Test]
        public void SetSpeed_ValidValue_UpdatesCurrentMultiplier()
        {
            // Act
            bus.Publish<SetSpeedCommand>(new SetSpeedCommand(5));

            // Assert
            Assert.That(service.CurrentMultiplier, Is.EqualTo(5));
        }

        [Test]
        public void PublishSetSpeedCommand_SameAsCurrent_DoesNotPublish()
        {
            // Arrange
            var received = new List<SpeedChanged>();
            using (bus.Subscribe<SpeedChanged>(evt => received.Add(evt)))
            {
                // Act
                bus.Publish(new SetSpeedCommand(1));

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(service.CurrentMultiplier, Is.EqualTo(1));
                    Assert.That(received, Is.Empty);
                });
            }
        }

        [Test]
        public void PublishSetSpeedCommand_NonPositive_DoesNotPublishOrChange()
        {
            // Arrange
            var received = new List<SpeedChanged>();
            using (bus.Subscribe<SpeedChanged>(evt => received.Add(evt)))
            {
                // Act
                bus.Publish(new SetSpeedCommand(0));
                bus.Publish(new SetSpeedCommand(-5));

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(service.CurrentMultiplier, Is.EqualTo(1));
                    Assert.That(received, Is.Empty);
                });
            }
        }
    }
}
