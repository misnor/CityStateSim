using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
using CityStateSim.Core.Enums;
using CityStateSim.Gameplay.Components;
using DefaultEcs;
using Microsoft.Extensions.Logging;

namespace CityStateSim.Gameplay.Commands;

public class CancelJobCommandHandler : ICommandHandler<CancelJobCommand>
{
    private readonly World world;
    private readonly ILogger<CancelJobCommandHandler> logger;

    public CancelJobCommandHandler(World world, ILogger<CancelJobCommandHandler> logger)
    {
        this.world = world;
        this.logger = logger;
    }

    public void Handle(CancelJobCommand command)
    {
        logger.LogInformation(
            "Cancelling jobs in area: ({MinX}, {MinY}) to ({MaxX}, {MaxY})",
            command.MinX, command.MinY, command.MaxX, command.MaxY);

        var jobs = world.GetEntities()
            .With<JobComponent>()
            .With<PositionComponent>()
            .AsEnumerable()
            .Where(e =>
            {
                var p = e.Get<PositionComponent>();
                return p.X >= command.MinX && p.X <= command.MaxX
                    && p.Y >= command.MinY && p.Y <= command.MaxY;
            });

        foreach (var jobEntity in jobs)
        {
            var pos = jobEntity.Get<PositionComponent>();

            var tile = world.GetEntities()
                .With<PositionComponent>()
                .With<TileTypeComponent>()
                .AsEnumerable()
                .FirstOrDefault(e =>
                {
                    var tp = e.Get<PositionComponent>();
                    return tp.X == pos.X && tp.Y == pos.Y;
                });

            if (tile.IsAlive && tile.Has<JobOverlayComponent>())
            {
                tile.Remove<JobOverlayComponent>();
            }

            if (jobEntity.Has<JobProgressComponent>())
            {
                var prog = jobEntity.Get<JobProgressComponent>();
                var agent = prog.AgentEntity;
                if (agent.IsAlive)
                {
                    agent.Set(new AgentStateComponent { State = AgentState.Idle });
                    agent.Remove<MovementIntentComponent>();
                }
                jobEntity.Remove<JobProgressComponent>();
            }

            jobEntity.Dispose();
            logger.LogDebug(
                "Cancelled job at ({X}, {Y}) and cleared overlay",
                pos.X, pos.Y);
        }
    }
}