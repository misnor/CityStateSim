using CityStateSim.Core.Components;
using CityStateSim.Core.Enums;
using DefaultEcs;

namespace CityStateSim.Gameplay.Jobs;
internal class DeliverResourceJobHandler : IJobHandler
{
    public JobType JobType => JobType.DeliverResource;

    public void HandleCompletion(World world, Entity agent, Entity jobEntity)
    {
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
