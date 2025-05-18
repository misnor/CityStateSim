using CityStateSim.Core.Components;
using CityStateSim.Core.Enums;
using CityStateSim.Gameplay.Components;
using CityStateSim.Gameplay.Jobs;
using CityStateSim.Gameplay.Services.Interfaces;
using CityStateSim.Gameplay.Simulation.Interfaces;
using DefaultEcs;

namespace CityStateSim.Gameplay.Simulation;
public class WorkProgressSystem : IWorldTickSystem
{
    private readonly IEnumerable<IJobHandler> handlers;
    private readonly ITickSpeedService tickSpeedService;

    public WorkProgressSystem(IEnumerable<IJobHandler> handlers,
        ITickSpeedService tickSpeedService)
    {
        this.handlers = handlers;
        this.tickSpeedService = tickSpeedService;
    }

    public void Update(World world)
    {
        // dt in seconds, adjusted by your tick‐speed multiplier
        float dt = tickSpeedService.CurrentMultiplier * (1f / 10f);

        var inProgressJobs = world.GetEntities()
            .With<JobComponent>()
            .With<JobProgressComponent>()
            .AsEnumerable()
            .ToArray();

        foreach (var jobEntity in inProgressJobs)
        {
            ref var progress = ref jobEntity.Get<JobProgressComponent>();
            progress.RemainingSeconds -= dt;

            if (progress.RemainingSeconds > 0)
            {
                jobEntity.Set(progress);
                continue;
            }

            // Jobs done
            var jobComp = jobEntity.Get<JobComponent>();
            var handler = handlers.First(h => h.JobType == jobComp.JobType);

            // Handle job specific stuff
            handler.HandleCompletion(world, progress.AgentEntity, jobEntity);

            //jobEntity.Dispose();
            var agent = progress.AgentEntity;
            agent.Set(new AgentStateComponent { State = AgentState.Idle });

            jobEntity.Remove<JobProgressComponent>();
        }
    }
}
