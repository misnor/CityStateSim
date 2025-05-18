using CityStateSim.Core.Commands;

namespace CityStateSim.Gameplay.Commands;
public record MoveCameraCommand(int DX, int DY) : ICommand;
