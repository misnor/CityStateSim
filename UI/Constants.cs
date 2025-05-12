namespace CityStateSim.UI;

/// <summary>
/// Central location for UI-related constants.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Size of a single tile in pixels.
    /// </summary>
    public const int TileSize = 32;

    /// <summary>
    /// Height of the toolbar button in pixels.
    /// </summary>
    public const int ToolbarButtonSize = 48;

    /// <summary>
    /// Vertical margin/padding for the toolbar in pixels.
    /// </summary>
    public const int ToolbarMarginY = 16;

    /// <summary>
    /// Total height of the toolbar area (button + margins).
    /// </summary>
    public const int ToolbarHeight = ToolbarButtonSize + ToolbarMarginY * 2;
} 