using CityStateSim.Core.Commands;

namespace CityStateSim.Gameplay.Commands;

public class CancelJobCommand : ICommand
{
    public int MinX { get; }
    public int MinY { get; }
    public int MaxX { get; }
    public int MaxY { get; }

    public CancelJobCommand(int minX, int minY, int maxX, int maxY)
    {
        MinX = minX;
        MinY = minY;
        MaxX = maxX;
        MaxY = maxY;
    }
} 