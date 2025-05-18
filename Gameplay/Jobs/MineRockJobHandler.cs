using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityStateSim.Core.Components;
using DefaultEcs;

namespace CityStateSim.Gameplay.Jobs;
internal class MineRockJobHandler : IJobHandler
{
    public JobType JobType => JobType.MineRock;

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
    }
}
