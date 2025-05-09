using Core.EventBus.Interfaces;
using DefaultEcs;

namespace Infrastructure.Events;
public class EcsEventBusAdapter : IEcsEventBus
{
    private readonly World world;

    public EcsEventBusAdapter(World world) => this.world = world;

    public void Publish<T>(T @event)
    {
        world.Publish(@event);
    }

    public IDisposable Subscribe<T>(Action<T> handler)
    {
        void Adapter(in T msg) => handler(msg);
        return world.Subscribe<T>(Adapter);
    }
}
