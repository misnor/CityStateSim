using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
using CityStateSim.Gameplay.Commands;
using DefaultEcs;

namespace CityStateSim.Gameplay.Handlers;
public class MarkRocksForMiningCommandHandler : ICommandHandler<MarkStonesForMiningCommand>
{
    private readonly World world;
    public MarkRocksForMiningCommandHandler(World world)
    {
        this.world = world;
    }

    public void Handle(MarkStonesForMiningCommand command)
    {
        // Ensure coordinates are ordered correctly
        int minX = Math.Min(command.StartX, command.EndX);
        int maxX = Math.Max(command.StartX, command.EndX);
        int minY = Math.Min(command.StartY, command.EndY);
        int maxY = Math.Max(command.StartY, command.EndY);

        // Find all stone entities in the rectangle
        var rocks = world.GetEntities()
            .With<PositionComponent>()
            .With<TileTypeComponent>()
            .AsEnumerable()
            .Where(e =>
            {
                ref var pos = ref e.Get<PositionComponent>();
                ref var type = ref e.Get<TileTypeComponent>();
                return type.Id == "rock" &&
                       pos.X >= minX && pos.X <= maxX &&
                       pos.Y >= minY && pos.Y <= maxY;
            });

        foreach (var rock in rocks)
        {
            rock.Set<MineRockJobTag>();
        }
    }
}
