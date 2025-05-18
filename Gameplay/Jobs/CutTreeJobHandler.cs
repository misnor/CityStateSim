using CityStateSim.Core.Components;
using DefaultEcs;

namespace CityStateSim.Gameplay.Jobs;
public class CutTreeJobHandler : IJobHandler
{
    public JobType JobType => JobType.HarvestTree;

    public void HandleCompletion(World world, Entity agent, Entity jobEntity)
    {
        var job = jobEntity.Get<JobComponent>();

        var tile = world.GetEntities()
            .With<PositionComponent>()
            .With<TileTypeComponent>()
            .AsEnumerable()
            .Single(e =>
            {
                var p = e.Get<PositionComponent>();
                return p.X == job.TargetX && p.Y == job.TargetY;
            });

        tile.Remove<TileTypeComponent>();
        tile.Set<TileTypeComponent>(new TileTypeComponent("grass"));

        // spawn wood for collection
/*        var resource = world.CreateEntity();
        resource.Set(new ResourceComponent
        {
            X = job.TargetX,
            Y = job.TargetY,
            ResourceType = "wood"
        });*/
    }
}
