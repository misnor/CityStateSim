using System.Linq;
using CityStateSim.Core.Components;
using CityStateSim.Core.Components.Tags;
using CityStateSim.Core.Enums;
using CityStateSim.Gameplay.Components;   // for CarryingComponent
using CityStateSim.Gameplay.Simulation.Interfaces;
using DefaultEcs;
using Microsoft.Extensions.Logging;

namespace CityStateSim.Gameplay.Simulation
{
    public class JobAssignmentSystem : IWorldTickSystem
    {
        private readonly ILogger<JobAssignmentSystem> logger;

        public JobAssignmentSystem(ILogger<JobAssignmentSystem> logger)
        {
            this.logger = logger;
        }

        public void Update(World world)
        {
            // gather all idle agents once
            var idleAgents = world.GetEntities()
                .With<AgentTag>()
                .With<AgentStateComponent>()
                .With<PositionComponent>()
                .AsEnumerable()
                .Where(a => a.Get<AgentStateComponent>().State == AgentState.Idle)
                .ToList();

            if (idleAgents.Count == 0)
                return;

            // gather all unassigned jobs
            var availableJobs = world.GetEntities()
                .With<JobComponent>()
                .With<PositionComponent>()
                .AsEnumerable()
                .Where(j => !j.Get<JobComponent>().IsAssigned)
                .ToList();

            if (availableJobs.Count == 0)
                return;

            foreach (var jobEntity in availableJobs)
            {
                var job = jobEntity.Get<JobComponent>();
                var jobPos = jobEntity.Get<PositionComponent>();

                // pick only the agents eligible for this job type
                var candidates = job.JobType switch
                {
                    JobType.PickupResource => idleAgents.Where(a => !a.Has<CarryingComponent>()),
                    JobType.DeliverResource => idleAgents.Where(a => a.Has<CarryingComponent>()),
                    _ => idleAgents
                };

                if (!candidates.Any())
                    continue;

                // find the nearest eligible agent
                var nearest = candidates
                    .OrderBy(a =>
                    {
                        var p = a.Get<PositionComponent>();
                        var dx = p.X - jobPos.X;
                        var dy = p.Y - jobPos.Y;
                        return dx * dx + dy * dy;
                    })
                    .First();

                if (!nearest.IsAlive)
                    continue;

                // mark job assigned
                job.IsAssigned = true;
                jobEntity.Set(job);

                // send agent walking
                ref var state = ref nearest.Get<AgentStateComponent>();
                state.State = AgentState.Walking;
                nearest.Set(new MovementIntentComponent(jobPos.X, jobPos.Y));

                // remove that agent from this batch so we don't double‐assign
                idleAgents.Remove(nearest);
            }
        }
    }
}
