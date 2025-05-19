
using CityStateSim.Core.Commands;

namespace CityStateSim.Gameplay.Commands;
public class CreateDeliverJobCommand : ICommand
{
    public int TargetX { get; }
    public int TargetY { get; }

    public CreateDeliverJobCommand(int targetX, int targetY)
    {
        TargetX = targetX;
        TargetY = targetY;
    }
}
