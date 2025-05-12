namespace CityStateSim.Core.Events;
public record TickOccurred;

public record SetSpeedCommand(int multiplier);

public record SpeedChanged(int newMultiplier);
