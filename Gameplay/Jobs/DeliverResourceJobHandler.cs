using CityStateSim.Core.Components;
using CityStateSim.Core.Enums;
using DefaultEcs;
using Microsoft.Extensions.Logging;

namespace CityStateSim.Gameplay.Jobs;
internal class DeliverResourceJobHandler : IJobHandler
{
    private readonly ILogger<DeliverResourceJobHandler> logger;

    public JobType JobType => JobType.DeliverResource;
    public DeliverResourceJobHandler(ILogger<DeliverResourceJobHandler> logger)
    {
        this.logger = logger;
    }

    public void HandleCompletion(World world, Entity agent, Entity jobEntity)
    {
        // This is a defensive check - for some reason some
        // Agents can get to a stockpile without a CarryingComponent
        if (!agent.Has<CarryingComponent>())
        {
            this.logger.LogError("An agent just tried to deliver nothing!");
            agent.Remove<MovementIntentComponent>();
            agent.Set(new AgentStateComponent { State = AgentState.Idle });
            jobEntity.Dispose();
            return;
        }

        // find the stockpile at the job tile
        var sp = world.GetEntities()
            .With<PositionComponent>()
            .With<StockpileComponent>()
            .AsEnumerable()
            .FirstOrDefault(e =>
            {
                var p = e.Get<PositionComponent>();
                var j = jobEntity.Get<JobComponent>();
                return p.X == j.TargetX && p.Y == j.TargetY;
            });

        if (sp.IsAlive)
        {
            // deposit one unit of whatever the agent is carrying
            var carry = agent.Get<CarryingComponent>();
            ref var stockpileInventory = ref sp.Get<StockpileComponent>();

            if (!stockpileInventory.Inventory.ContainsKey(carry.ResourceType))
            { 
                stockpileInventory.Inventory[carry.ResourceType] = 0; 
            }

            stockpileInventory.Inventory[carry.ResourceType] += carry.Quantity;
        }

        // cleanup
        agent.Remove<CarryingComponent>();
        jobEntity.Dispose();
        agent.Set(new AgentStateComponent { State = AgentState.Idle });
    }
}
