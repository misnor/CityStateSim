using DefaultEcs;

namespace CityStateSim.Gameplay.Components;
public struct JobProgressComponent
{
    public float RemainingSeconds;
    public Entity AgentEntity;
}
