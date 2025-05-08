using Core.EventBus.Interfaces;
using DefaultEcs;

namespace Infrastructure.Events;
public class EcsEventBusAdapter : IEcsEventBus
{
    private readonly World _world;

    public EcsEventBusAdapter(World world) => _world = world;

    public void Publish<T>(T @event)
    {
        _world.Publish(@event);
    }

    public IDisposable Subscribe<T>(Action<T> handler)
    {
        void Adapter(in T msg) => handler(msg);
        return _world.Subscribe<T>(Adapter);
    }
}
