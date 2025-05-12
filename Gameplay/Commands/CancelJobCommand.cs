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
        if (minX > maxX)
        {
            throw new ArgumentException("MinX cannot be greater than MaxX", nameof(minX));
        }
        if (minY > maxY)
        {
            throw new ArgumentException("MinY cannot be greater than MaxY", nameof(minY));
        }

        MinX = minX;
        MinY = minY;
        MaxX = maxX;
        MaxY = maxY;
    }
} 