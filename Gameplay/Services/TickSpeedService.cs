using CityStateSim.Core.EventBus.Interfaces;
using CityStateSim.Core.Events;
using CityStateSim.Gameplay.Services.Interfaces;

namespace CityStateSim.Gameplay.Services;
public class TickSpeedService : ITickSpeedService
{
    private int currentMultiplier = 1;
    public int CurrentMultiplier => currentMultiplier;

    public TickSpeedService(IAppEventBus appBus)
    {
        appBus.Subscribe<SetSpeedCommand>(cmd =>
        {
            if (cmd.multiplier > 0 && cmd.multiplier != currentMultiplier)
            {
                currentMultiplier = cmd.multiplier;
                appBus.Publish(new SpeedChanged(currentMultiplier));
            }
        });
    }

    public void SetSpeed(int multiplier)
    {
        if (multiplier < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(multiplier), "Multiplier must be at least 1.");
        }

        currentMultiplier = multiplier;
    }
}
