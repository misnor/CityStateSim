using CityStateSim.Core.Commands;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
using DefaultEcs;

namespace CityStateSim.Gameplay.Handlers;

public class MarkTreesForCuttingCommandHandler : ICommandHandler<MarkTreesForCuttingCommand>
{
    private readonly World world;
    
    public MarkTreesForCuttingCommandHandler(World world)
    {
        this.world = world;
    }
    
    public void Handle(MarkTreesForCuttingCommand command)
    {
        // Ensure coordinates are ordered correctly
        int minX = Math.Min(command.StartX, command.EndX);
        int maxX = Math.Max(command.StartX, command.EndX);
        int minY = Math.Min(command.StartY, command.EndY);
        int maxY = Math.Max(command.StartY, command.EndY);

        // Find all tree entities in the rectangle
        var trees = world.GetEntities()
            .With<PositionComponent>()
            .With<TileTypeComponent>()
            .AsEnumerable()
            .Where(e =>
            {
                ref var pos = ref e.Get<PositionComponent>();
                ref var type = ref e.Get<TileTypeComponent>();
                return type.Id == "tree" &&
                       pos.X >= minX && pos.X <= maxX &&
                       pos.Y >= minY && pos.Y <= maxY;
            });

        foreach (var tree in trees)
        {
            var jobEntity = world.CreateEntity();
            var jobPos = tree.Get<PositionComponent>();
            jobEntity.Set(new PositionComponent(jobPos.X, jobPos.Y));
            jobEntity.Set(new JobComponent(jobPos.X, jobPos.Y, JobType.HarvestTree) { Status = JobStatus.Pending });
            tree.Set(new JobOverlayComponent());
        }
    }
} 