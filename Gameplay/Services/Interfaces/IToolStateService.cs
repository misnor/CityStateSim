namespace CityStateSim.Gameplay.Services.Interfaces;

public enum ToolType
{
    None,
    Axe,
    Pickaxe,
    Cancel
}

public interface IToolStateService
{
    ToolType CurrentTool { get; set; }
} 