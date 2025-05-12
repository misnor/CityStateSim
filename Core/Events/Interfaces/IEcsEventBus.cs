namespace CityStateSim.Core.EventBus.Interfaces;

public interface IEcsEventBus
{
    void Publish<T>(T evt);
    IDisposable Subscribe<T>(Action<T> handler);
}
