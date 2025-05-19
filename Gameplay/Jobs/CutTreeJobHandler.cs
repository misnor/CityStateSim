using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Core.Enums;
using CityStateSim.Gameplay.Commands;
using DefaultEcs;

namespace CityStateSim.Gameplay.Jobs;
public class CutTreeJobHandler : IJobHandler
{
    private ICommandDispatcher dispatcher;
    
    public CutTreeJobHandler(ICommandDispatcher dispatcher)
    {
        this.dispatcher = dispatcher;
    }
    
    public JobType JobType => JobType.HarvestTree;

    public void HandleCompletion(World world, Entity agent, Entity jobEntity)
    {
        var job = jobEntity.Get<JobComponent>();

        // 1) Change the tile _entity_ to grass
        var tileEntity = world.GetEntities()
            .With<PositionComponent>()
            .With<TileTypeComponent>()
            .AsEnumerable()
            .Single(e =>
            {
                var p = e.Get<PositionComponent>();
                return p.X == job.TargetX && p.Y == job.TargetY;
            });

        if (tileEntity.Has<JobOverlayComponent>())
        {
            tileEntity.Remove<JobOverlayComponent>();
        }

        tileEntity.Set(new TileTypeComponent("grass"));

        // 2) Spawn _a new_ entity for the wood resource
        var resourceEntity = world.CreateEntity();
        resourceEntity.Set(new PositionComponent(job.TargetX, job.TargetY));
        resourceEntity.Set(new ResourceComponent
        {
            ResourceType = "wood",
            Quantity = 1
        });

        // 3) Kick off pickup
        dispatcher.Dispatch(new CreatePickupJobCommand(
            job.TargetX, job.TargetY));

        // 4) Clean up the cut‐tree job
        jobEntity.Dispose();

        // 5) Reset the agent
        agent.Set(new AgentStateComponent { State = AgentState.Idle });
    }
}
