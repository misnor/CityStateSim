using CityStateSim.Core.Components.Tags;
using CityStateSim.Core.Components;
using CityStateSim.Core.Enums;
using CityStateSim.Gameplay.Simulation.Interfaces;
using DefaultEcs;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;

namespace CityStateSim.Gameplay.Simulation;
public class JobAssignmentSystem : IWorldTickSystem
{
    ILogger<JobAssignmentSystem> logger;
    
    public JobAssignmentSystem(ILogger<JobAssignmentSystem> logger)
    {
        this.logger = logger;
    }

    public void Update(World world)
    {
        var idleAgents = world.GetEntities()
            .With<AgentTag>()
            .With<AgentStateComponent>()
            .With<PositionComponent>()
            .AsEnumerable()
            .Where(e =>
                e.Get<AgentStateComponent>().State == AgentState.Idle
            )
            .ToArray();

        var availableJobs = world.GetEntities()
            .With<JobComponent>()
            .With<PositionComponent>()
            .AsEnumerable()
            .Where(x =>
            {
                var jobComponent = x.Get<JobComponent>();
                return !jobComponent.IsAssigned;
            })
            .ToArray();

        if (availableJobs.Length == 0)
        {
            return;
        }

        if(idleAgents.Length == 0)
        {
            return;
        }

        foreach (var jobEntity in availableJobs)
        {
            var job = jobEntity.Get<JobComponent>();
            var jobPos = jobEntity.Get<PositionComponent>();

            var nearestAgent = idleAgents
                .OrderBy(e =>
                {
                    var pos = e.Get<PositionComponent>();
                    var dx = pos.X - jobPos.X;
                    var dy = pos.Y - jobPos.Y;
                    return dx * dx + dy * dy;
                })
                .FirstOrDefault();

            if(!nearestAgent.IsAlive)
            {
                return;
            }

                // mark job assigned
            job.IsAssigned = true;
            jobEntity.Set(job);
            ref var agentState = ref nearestAgent.Get<AgentStateComponent>();

            agentState.State = AgentState.Walking;
            // give agent a MovementIntent + update its state
            nearestAgent.Set(new MovementIntentComponent(jobPos.X, jobPos.Y));
            nearestAgent.Set(new AgentStateComponent { State = AgentState.Walking });

                // remove that agent from future consideration
            idleAgents = idleAgents.Except(new[] { nearestAgent }).ToArray();
        }
    }
}
