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
        logger.LogInformation("Cancelling jobs in area: ({MinX}, {MinY}) to ({MaxX}, {MaxY})",
            command.MinX, command.MinY, command.MaxX, command.MaxY);

        var entities = world.GetEntities()
            .With<PositionComponent>()
            .AsEnumerable();

        foreach (var entity in world.GetEntities()
                                                .With<PositionComponent>()
                                                .AsEnumerable())
        {
            var pos = entity.Get<PositionComponent>();
            if (pos.X < command.MinX || pos.X > command.MaxX ||
                pos.Y < command.MinY || pos.Y > command.MaxY)
            {
                continue;
            }

            if (!entity.Has<JobComponent>())
            {
                continue;
            }

            if (entity.Has<JobProgressComponent>())
            {
                var prog = entity.Get<JobProgressComponent>();
                var agent = prog.AgentEntity;

                if (agent.IsAlive)
                {
                    agent.Set(new AgentStateComponent { State = AgentState.Idle });
                    agent.Remove<MovementIntentComponent>();
                }

                entity.Remove<JobProgressComponent>();
            }

            entity.Remove<JobComponent>();
            logger.LogDebug("Removed JobComponent at ({X}, {Y})", pos.X, pos.Y);
        }
    }
} 