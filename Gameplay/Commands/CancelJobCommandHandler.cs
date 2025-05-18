using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
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

        // Find all entities with positions in the rectangle
        var entities = world.GetEntities()
            .With<PositionComponent>()
            .AsEnumerable();

        foreach (var entity in entities)
        {
            var pos = entity.Get<PositionComponent>();
            if (pos.X >= command.MinX && pos.X <= command.MaxX &&
                pos.Y >= command.MinY && pos.Y <= command.MaxY)
            {
                if (entity.Has<JobComponent>())
                {
                    entity.Remove<JobComponent>();
                    logger.LogDebug("Removed JobComponent from entity at ({X}, {Y})", pos.X, pos.Y);
                }
            }
        }
    }
} 