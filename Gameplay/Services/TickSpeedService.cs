using CityStateSim.Core.EventBus.Interfaces;
using CityStateSim.Core.Events;
using CityStateSim.Gameplay.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CityStateSim.Gameplay.Services;
public class TickSpeedService : ITickSpeedService
{
    private int currentMultiplier = 1;
    private readonly ILogger<TickSpeedService> logger;

    public int CurrentMultiplier => currentMultiplier;

    public TickSpeedService(IAppEventBus bus, ILogger<TickSpeedService> logger)
    {
        bus.Subscribe<SetSpeedCommand>(OnSetSpeedEvent);
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private void OnSetSpeedEvent(SetSpeedCommand t)
    {
        SetSpeed(t.multiplier);
    }

    private void SetSpeed(int multiplier)
    {
        if (multiplier < 1)
        {
            this.logger.LogWarning("Attempted to set the speed to a negative numbers: {Multiplier}", multiplier);
            return;
        }

        currentMultiplier = multiplier;
    }
}
