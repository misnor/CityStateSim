using CityStateSim.Core.Commands;

namespace CityStateSim.Gameplay.Commands;
public class CreatePickupJobCommand : ICommand
{
    public int X { get; }
    public int Y { get; }

    public CreatePickupJobCommand(int x, int y)
    {
        X = x;
        Y = y;
    }
}
