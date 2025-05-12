namespace CityStateSim.Gameplay.Simulation.Interfaces;

/// <summary>
/// Drives the per-tick execution of all registered IWorldTickSystems, 
/// and publishes the standard TickOccurred event
/// </summary>
public interface ISimulationRunner
{
    /// <summary>
    /// Advance the simulation by one logical tick.
    /// </summary>
    void Tick();

    void TogglePause();

    bool IsPaused { get; }
}
