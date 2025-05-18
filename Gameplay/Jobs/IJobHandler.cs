using CityStateSim.Core.Components;
using DefaultEcs;

namespace CityStateSim.Gameplay.Jobs;
public interface IJobHandler
{
    JobType JobType { get; }
    void HandleCompletion(World world, Entity agent, Entity jobEntity);
}
