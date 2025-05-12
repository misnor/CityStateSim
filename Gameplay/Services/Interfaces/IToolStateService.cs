namespace CityStateSim.Gameplay.Services.Interfaces;

public enum ToolType
{
    None,
    Axe
}

public interface IToolStateService
{
    ToolType CurrentTool { get; set; }
} 