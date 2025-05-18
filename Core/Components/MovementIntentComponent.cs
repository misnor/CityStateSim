namespace CityStateSim.Core.Components;

public struct MovementIntentComponent
{
    public MovementIntentComponent(int targetX, int targetY) : this()
    {
        TargetX = targetX;
        TargetY = targetY;
        AccumulatorX = 0;
        AccumulatorY = 0;
    }

    public int TargetX { get; set; }
    public int TargetY { get; set; }
    public float AccumulatorX { get; set; }
    public float AccumulatorY { get; set; }
}
