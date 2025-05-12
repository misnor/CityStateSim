namespace CityStateSim.Infrastructure.Config.Interfaces;

/// <summary>
/// Abstraction for loading JSON (or other) configuration files.
/// </summary>
public interface IConfigProvider
{
    /// <summary>
    /// Load and deserialize a configuration file into the specified type.
    /// </summary>
    /// <typeparam name="T">Target data type</typeparam>
    /// <param name="fileName">Relative file path under the Config folder (e.g. "tiles.json").</param>
    public T LoadConfig<T>(string fileName);
}
