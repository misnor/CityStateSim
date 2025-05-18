using System;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.UI.Rendering.Interfaces;
using CityStateSim.UI.Factories.Interfaces;
using CityStateSim.Gameplay.Services.Interfaces;
using CityStateSim.Infrastructure.Input;
using CityStateSim.Gameplay.Commands;
using CityStateSim.Core.Commands;
using CityStateSim.UI.Camera;
using Microsoft.Xna.Framework.Content;
using Microsoft.Extensions.Logging;
using CityStateSim.UI;

namespace CityStateSim.UI.Rendering;

public class RectangleDrawSystem : IRenderSystem
{
    private readonly ITextureFactory textureFactory;
    private readonly IInputService inputService;
    private readonly IToolStateService toolStateService;
    private readonly ICommandDispatcher commandDispatcher;
    private readonly Camera2D camera;
    private readonly ILogger<RectangleDrawSystem> logger;
    private Texture2D pixel;
    private Vector2? startPosition;
    private Vector2? currentPosition;
    private bool wasMouseDown;

    public RectangleDrawSystem(
        ITextureFactory textureFactory,
        IInputService inputService,
        IToolStateService toolStateService,
        ICommandDispatcher commandDispatcher,
        Camera2D camera,
        ILogger<RectangleDrawSystem> logger)
    {
        this.textureFactory = textureFactory ?? throw new ArgumentNullException(nameof(textureFactory));
        this.inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
        this.toolStateService = toolStateService ?? throw new ArgumentNullException(nameof(toolStateService));
        this.commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        this.camera = camera ?? throw new ArgumentNullException(nameof(camera));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        pixel = textureFactory.GetTexture("whitePixel");
    }

    public void Draw(SpriteBatch spriteBatch, World world)
    {
        if (toolStateService.CurrentTool != ToolType.Axe && toolStateService.CurrentTool != ToolType.Cancel)
        {
            return;
        }

        bool isMouseDown = inputService.IsMouseButtonDown(MouseButton.Left);
        var mousePos = inputService.GetMousePosition();
        var screenPos = new Vector2(mousePos.X, mousePos.Y);
        var worldPos = camera.ScreenToWorld(screenPos, spriteBatch.GraphicsDevice);

        // Start drawing
        if (isMouseDown && !wasMouseDown)
        {
            startPosition = worldPos;
            currentPosition = startPosition;
            logger.LogInformation("Started drawing at world position: {X}, {Y}", startPosition.Value.X, startPosition.Value.Y);
        }
        // Update while drawing
        else if (isMouseDown && startPosition.HasValue)
        {
            currentPosition = worldPos;
        }
        // End drawing
        else if (!isMouseDown && startPosition.HasValue)
        {
            if (currentPosition.HasValue)
            {
                // Convert world coordinates to tile coordinates
                var startTile = WorldToTile(startPosition.Value);
                var endTile = WorldToTile(currentPosition.Value);

                // Calculate min/max coordinates
                int minX = Math.Min(startTile.X, endTile.X);
                int maxX = Math.Max(startTile.X, endTile.X);
                int minY = Math.Min(startTile.Y, endTile.Y);
                int maxY = Math.Max(startTile.Y, endTile.Y);

                logger.LogInformation("Ended drawing at world position: {X}, {Y}", currentPosition.Value.X, currentPosition.Value.Y);
                logger.LogInformation("Tile range: ({MinX}, {MinY}) to ({MaxX}, {MaxY})", minX, minY, maxX, maxY);

                // Create rectangle command based on tool type
                if (toolStateService.CurrentTool == ToolType.Axe)
                {
                    var command = new MarkTreesForCuttingCommand(minX, minY, maxX, maxY);
                    commandDispatcher.Dispatch(command);
                }
                else if (toolStateService.CurrentTool == ToolType.Cancel)
                {
                    var command = new CancelJobCommand(minX, minY, maxX, maxY);
                    commandDispatcher.Dispatch(command);
                }
            }

            startPosition = null;
            currentPosition = null;
        }

        wasMouseDown = isMouseDown;

        // Draw the rectangle if we're currently drawing
        if (startPosition.HasValue && currentPosition.HasValue)
        {
            DrawRectangle(spriteBatch, startPosition.Value, currentPosition.Value);
        }
    }

    private bool IsOffscreen(MousePosition pos, Viewport vp)
    {
        return pos.X < 0 || pos.X > vp.Width || pos.Y < 0 || pos.Y > vp.Height;
    }

    private bool IsOverToolbar(MousePosition pos, Viewport vp)
    {
        return pos.Y > vp.Height - Constants.ToolbarHeight;
    }

    private void DrawRectangle(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
    {
        float minX = Math.Min(start.X, end.X);
        float maxX = Math.Max(start.X, end.X);
        float minY = Math.Min(start.Y, end.Y);
        float maxY = Math.Max(start.Y, end.Y);

        var rect = new Rectangle(
            (int)minX,
            (int)minY,
            (int)(maxX - minX),
            (int)(maxY - minY)
        );

        // Draw semi-transparent yellow rectangle
        spriteBatch.Draw(pixel, rect, Color.Yellow * 0.5f);
    }

    private Point WorldToTile(Vector2 worldPos)
    {
        // Convert world coordinates to tile coordinates
        int tileX = (int)(worldPos.X / Constants.TileSize);
        int tileY = (int)(worldPos.Y / Constants.TileSize);
        return new Point(tileX, tileY);
    }
} 