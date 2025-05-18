using CityStateSim.Core.Components;
using CityStateSim.Core.Enums;
using CityStateSim.Gameplay.Components;
using CityStateSim.Gameplay.Simulation.Interfaces;
using DefaultEcs;

namespace CityStateSim.Gameplay.Simulation;
public class WorkStartSystem : IWorldTickSystem
{
    private readonly float workDurationInSeconds;

    public WorkStartSystem()
    {
        workDurationInSeconds = 2.5f;
    }

    public void Update(World world)
    {
        var arrivedAgents = world.GetEntities()
            .With<AgentStateComponent>()
            .With<MovementIntentComponent>()
            .With<PositionComponent>()
            .AsEnumerable()
            .Where(e =>
            {
                var state = e.Get<AgentStateComponent>().State;
                if (state != AgentState.Walking)
                {
                    return false;
                }

                var pos = e.Get<PositionComponent>();
                var intent = e.Get<MovementIntentComponent>();

                return pos.X == intent.TargetX && pos.Y == intent.TargetY;
            });

        foreach(var agent in arrivedAgents)
        {
            var pos = agent.Get<PositionComponent>();
            var intent = agent.Get<MovementIntentComponent>();

            var jobEntity = world.GetEntities()
                .With<JobComponent>()
                .With<PositionComponent>()
                .Without<JobProgressComponent>()
                .AsEnumerable()
                .SingleOrDefault(e =>
                {
                    var p = e.Get<PositionComponent>();
                    return p.X == pos.X && p.Y == pos.Y;
                });

            if(!jobEntity.IsAlive)
            {
                continue;
            }

            ref var state = ref agent.Get<AgentStateComponent>();
            state.State = AgentState.Working;
            agent.Remove<MovementIntentComponent>();

            jobEntity.Set(new JobProgressComponent()
            {
                RemainingSeconds = workDurationInSeconds,
                AgentEntity = agent
            });
        }
    }
}
