using CityStateSim.Core.Commands;

namespace CityStateSim.Core.Events;
public record TickOccurred;

public record SetSpeedCommand(int multiplier) : ICommand;

public record SpeedChanged(int newMultiplier) : ICommand;
