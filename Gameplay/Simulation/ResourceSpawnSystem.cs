using CityStateSim.Core.Components;
using CityStateSim.Gameplay.Simulation.Interfaces;
using DefaultEcs;

namespace CityStateSim.Gameplay.Simulation;
public class ResourceSpawnSystem : IWorldTickSystem
{

    public void Update(World world)
    {
        var pickups = world.GetEntities()
             .With<ResourceComponent>()
             .With<PositionComponent>()
             .Without<JobComponent>()
             .AsEnumerable();

        foreach (var res in pickups)
        {
            var pos = res.Get<PositionComponent>();

            var stockpile = GetNearestStockpile(world, pos.X, pos.Y);
            if (!stockpile.IsAlive)
                continue;

            var spPos = stockpile.Get<PositionComponent>();

            res.Set(new JobComponent(
                spPos.X,
                spPos.Y,
                JobType.HaulResource
            ));
        }
    }

    private Entity GetNearestStockpile(World world, int x, int y)
    {
        Entity nearest = default;
        int bestDistSq = int.MaxValue;

        foreach (var e in world.GetEntities()
                               .With<PositionComponent>()
                               .With<StockpileComponent>()
                               .AsEnumerable())
        {
            var p = e.Get<PositionComponent>();
            int dx = p.X - x, dy = p.Y - y;
            int distSq = dx * dx + dy * dy;
            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                nearest = e;
            }
        }

        return nearest;
    }
}
