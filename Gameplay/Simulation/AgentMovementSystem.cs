using CityStateSim.Core.Components;
using CityStateSim.Gameplay.Simulation.Interfaces;
using DefaultEcs;

namespace CityStateSim.Gameplay.Simulation;
public class AgentMovementSystem : IWorldTickSystem
{
    private readonly float speed;

    public AgentMovementSystem()
    {
        speed = 0.25f;
    }

    public void Update(World world)
    {
        foreach (var entity in world.GetEntities()
                                    .With<PositionComponent>()
                                    .With<MovementIntentComponent>()
                                    .AsEnumerable())
        {
            var pos = entity.Get<PositionComponent>();
            var intent = entity.Get<MovementIntentComponent>();

            var deltaX = intent.TargetX - pos.X;
            var deltaY = intent.TargetY - pos.Y;
            var length = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (length > 0)
            {
                intent.AccumulatorX += deltaX / length * speed;
                intent.AccumulatorY += deltaY / length * speed;

                var stepX = (int)intent.AccumulatorX;
                var stepY = (int)intent.AccumulatorY;

                intent.AccumulatorX -= stepX;
                intent.AccumulatorY -= stepY;

                pos.X += stepX;
                pos.Y += stepY;

                entity.Set(pos);
                entity.Set(intent);
            }

/*            if (pos.X == intent.TargetX
                && pos.Y == intent.TargetY)
            {
                entity.Remove<MovementIntentComponent>();
            }*/
        }
    }
}
