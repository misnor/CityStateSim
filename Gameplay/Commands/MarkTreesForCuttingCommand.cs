using CityStateSim.Core.Commands;

namespace CityStateSim.Gameplay.Commands;

public record MarkTreesForCuttingCommand(int StartX, int StartY, int EndX, int EndY) : ICommand; 