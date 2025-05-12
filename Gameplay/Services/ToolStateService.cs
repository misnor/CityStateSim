using CityStateSim.Gameplay.Services.Interfaces;

namespace CityStateSim.Gameplay.Services;

public class ToolStateService : IToolStateService
{
    public ToolType CurrentTool { get; set; } = ToolType.None;
} 