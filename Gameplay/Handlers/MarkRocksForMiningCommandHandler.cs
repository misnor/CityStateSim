using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Gameplay.Commands;
using DefaultEcs;

namespace CityStateSim.Gameplay.Handlers
{
    public class MarkRocksForMiningCommandHandler : ICommandHandler<MarkStonesForMiningCommand>
    {
        private readonly World world;
        public MarkRocksForMiningCommandHandler(World world)
        {
            this.world = world;
        }

        public void Handle(MarkStonesForMiningCommand command)
        {
            int minX = Math.Min(command.StartX, command.EndX);
            int maxX = Math.Max(command.StartX, command.EndX);
            int minY = Math.Min(command.StartY, command.EndY);
            int maxY = Math.Max(command.StartY, command.EndY);

            var rocks = world.GetEntities()
                .With<PositionComponent>()
                .With<TileTypeComponent>()
                .AsEnumerable()
                .Where(e =>
                {
                    ref var pos = ref e.Get<PositionComponent>();
                    ref var type = ref e.Get<TileTypeComponent>();
                    return type.Id == "rock"
                        && pos.X >= minX && pos.X <= maxX
                        && pos.Y >= minY && pos.Y <= maxY;
                });

            foreach (var rockTile in rocks)
            {
                var pos = rockTile.Get<PositionComponent>();

                var jobEntity = world.CreateEntity();
                jobEntity.Set(new PositionComponent(pos.X, pos.Y));
                jobEntity.Set(new JobComponent(pos.X, pos.Y, JobType.MineRock)
                {
                    Status = JobStatus.Pending
                });

                rockTile.Set(new JobOverlayComponent());
            }
        }
    }
}
