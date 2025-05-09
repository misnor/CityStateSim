using Core.EventBus.Interfaces;
using Core.Events;
using Gameplay.Services.Interfaces;

namespace Gameplay.Services;
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
