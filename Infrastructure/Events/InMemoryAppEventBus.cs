using System.Collections.Concurrent;
using CityStateSim.Core.EventBus.Interfaces;

namespace CityStateSim.Infrastructure.Events;
public class InMemoryAppEventBus : IAppEventBus 
{
    private readonly ConcurrentDictionary<Type, List<Delegate>> handlers = new();

    public void Publish<T>(T evt)
    {
        if(handlers.TryGetValue(typeof(T), out var delegates))
        {
            foreach (var handler in delegates)
            {
                ((Action<T>)handler)(evt);
            }
        }
    }

    public IDisposable Subscribe<T>(Action<T> handler)
    {
        var list = handlers.GetOrAdd(typeof(T), _ => new List<Delegate>());
        list.Add(handler);
        return new Subscription(() => list.Remove(handler));
    }

    private class Subscription : IDisposable
    {
        private readonly Action unsubscribe;

        public Subscription(Action unsubscribe) => this.unsubscribe = unsubscribe;
        public void Dispose() => unsubscribe();
    }
}
