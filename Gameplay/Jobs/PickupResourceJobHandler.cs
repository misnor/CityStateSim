using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Core.Enums;
using CityStateSim.Gameplay.Commands;
using DefaultEcs;

namespace CityStateSim.Gameplay.Jobs;
public class PickupResourceJobHandler : IJobHandler
{
    public JobType JobType => JobType.PickupResource;

    private readonly ICommandDispatcher dispatcher;

    public PickupResourceJobHandler(ICommandDispatcher dispatcher)
    {
        this.dispatcher = dispatcher;
    }

    public void HandleCompletion(World world, Entity agent, Entity jobEntity)
    {
        var job = jobEntity.Get<JobComponent>();

        // 1) Find exactly the resource _entity_ at this location
        var resourceEntity = world.GetEntities()
            .With<ResourceComponent>()
            .With<PositionComponent>()
            .AsEnumerable()
            .FirstOrDefault(e =>
            {
                var p = e.Get<PositionComponent>();
                return p.X == job.TargetX && p.Y == job.TargetY;
            });

        if (resourceEntity.IsAlive)
        {
            // 2) Give the agent the carry component
            var rc = resourceEntity.Get<ResourceComponent>();
            agent.Set(new CarryingComponent
            {
                ResourceType = rc.ResourceType,
                Quantity = rc.Quantity
            });

            // 3) Remove the resource _entity_ (not the tile)
            resourceEntity.Dispose();
        }

        // 4) Enqueue the delivery job
        var stock = GetNearestStockpile(world, job.TargetX, job.TargetY);
        if (stock.IsAlive)
        {
            var spPos = stock.Get<PositionComponent>();
            dispatcher.Dispatch(new CreateDeliverJobCommand(
                spPos.X, spPos.Y));
        }

        // 5) Clean up the pickup‐job
        jobEntity.Dispose();

        // 6) Reset agent so it can get the next (deliver) job
        agent.Set(new AgentStateComponent { State = AgentState.Idle });
    }

    private Entity GetNearestStockpile(World world, int x, int y)
    {
        Entity best = default;
        int bestDist = int.MaxValue;
        foreach (var e in world.GetEntities()
                               .With<PositionComponent>()
                               .With<StockpileComponent>()
                               .AsEnumerable())
        {
            var p = e.Get<PositionComponent>();
            int dx = p.X - x, dy = p.Y - y, d2 = dx * dx + dy * dy;
            if (d2 < bestDist) { bestDist = d2; best = e; }
        }
        return best;
    }
}
