using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
using CityStateSim.Core.Enums;
using CityStateSim.Gameplay.Commands;
using DefaultEcs;

namespace CityStateSim.Gameplay.Jobs
{
    internal class MineRockJobHandler : IJobHandler
    {
        private readonly ICommandDispatcher dispatcher;

        public MineRockJobHandler(ICommandDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

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

            if (tile.Has<JobOverlayComponent>())
            {
                tile.Remove<JobOverlayComponent>();
            }

            tile.Set(new TileTypeComponent("grass"));

            var resourceEntity = world.CreateEntity();
            resourceEntity.Set(new PositionComponent(job.TargetX, job.TargetY));
            resourceEntity.Set(new ResourceComponent
            {
                ResourceType = "stone",
                Quantity = 1
            });

            dispatcher.Dispatch(new CreatePickupJobCommand(job.TargetX, job.TargetY));

            jobEntity.Dispose();

            ref var agentState = ref agent.Get<AgentStateComponent>();
            agentState.State = AgentState.Idle;
        }
    }
}
