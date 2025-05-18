using CityStateSim.Core.Enums;

namespace CityStateSim.Core.Components;

public enum JobType
{
    None,
    HarvestTree,
    MineRock,
}

public struct JobComponent
{
    public JobComponent(int targetX, int targetY, JobType jobType) : this()
    {
        TargetX = targetX;
        TargetY = targetY;
        JobType = jobType;
        IsAssigned = false;
    }

    public int TargetX { get; set; }
    public int TargetY { get; set; }
    public JobType JobType { get; set; }
    public bool IsAssigned { get; set; }
}