namespace CityStateSim.Core.EventBus.Interfaces;
public interface IAppEventBus
{
    void Publish<T>(T evt);
    IDisposable Subscribe<T>(Action<T> handler);
}
