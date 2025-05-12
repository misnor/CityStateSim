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
    private bool isDrawing;
    private bool wasMouseDown;
    private Vector2 startPos;
    private Vector2 currentPos;

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
        if (toolStateService.CurrentTool != ToolType.Axe)
        {
            isDrawing = false;
            wasMouseDown = false;
            return;
        }

        var mousePos = inputService.GetMousePosition();
        var viewport = spriteBatch.GraphicsDevice.Viewport;

        // Check if mouse is outside window or in toolbar
        if (IsOffscreen(mousePos, viewport) || IsOverToolbar(mousePos, viewport))
        {
            isDrawing = false;
            wasMouseDown = false;
            return;
        }

        var worldPos = camera.ScreenToWorld(new Vector2(mousePos.X, mousePos.Y), spriteBatch.GraphicsDevice, false);
        
        // Convert world position to tile coordinates
        var tilePos = new Point(
            (int)Math.Floor(worldPos.X / Constants.TileSize),
            (int)Math.Floor(worldPos.Y / Constants.TileSize)
        );

        logger.LogDebug("Mouse: ({MouseX}, {MouseY}) -> World: ({WorldX}, {WorldY}) -> Tile: ({TileX}, {TileY})",
            mousePos.X, mousePos.Y, worldPos.X, worldPos.Y, tilePos.X, tilePos.Y);

        bool isMouseDown = inputService.IsMouseButtonDown(MouseButton.Left);

        // Start drawing on mouse down
        if (isMouseDown && !wasMouseDown)
        {
            isDrawing = true;
            startPos = new Vector2(tilePos.X, tilePos.Y);
            currentPos = startPos;
            logger.LogInformation("Started drawing at tile position: {X}, {Y}", tilePos.X, tilePos.Y);
        }

        // Update current position while drawing
        if (isDrawing)
        {
            currentPos = new Vector2(tilePos.X, tilePos.Y);
            logger.LogDebug("Current position: {X}, {Y}", tilePos.X, tilePos.Y);

            // Draw the rectangle
            DrawRectangle(spriteBatch, startPos, currentPos);

            // End drawing on mouse up
            if (!isMouseDown)
            {
                isDrawing = false;
                logger.LogInformation("Ended drawing at tile position: {X}, {Y}", tilePos.X, tilePos.Y);
                commandDispatcher.Dispatch(new MarkTreesForCuttingCommand(
                    (int)startPos.X,
                    (int)startPos.Y,
                    (int)currentPos.X,
                    (int)currentPos.Y
                ));
            }
        }

        wasMouseDown = isMouseDown;
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
        // Calculate rectangle bounds
        float minX = Math.Min(start.X, end.X);
        float maxX = Math.Max(start.X, end.X);
        float minY = Math.Min(start.Y, end.Y);
        float maxY = Math.Max(start.Y, end.Y);

        // Convert tile coordinates to world coordinates
        var worldStart = new Vector2(minX * Constants.TileSize, minY * Constants.TileSize);
        var worldEnd = new Vector2((maxX + 1) * Constants.TileSize, (maxY + 1) * Constants.TileSize);

        // Convert world coordinates to screen coordinates
        var screenStart = camera.WorldToScreen(worldStart, spriteBatch.GraphicsDevice, false);
        var screenEnd = camera.WorldToScreen(worldEnd, spriteBatch.GraphicsDevice, false);

        logger.LogDebug("Drawing rectangle from world ({StartX}, {StartY}) to ({EndX}, {EndY}) -> screen ({ScreenStartX}, {ScreenStartY}) to ({ScreenEndX}, {ScreenEndY})",
            worldStart.X, worldStart.Y, worldEnd.X, worldEnd.Y,
            screenStart.X, screenStart.Y, screenEnd.X, screenEnd.Y);

        // Draw rectangle outline
        var rect = new Rectangle(
            (int)screenStart.X,
            (int)screenStart.Y,
            (int)(screenEnd.X - screenStart.X),
            (int)(screenEnd.Y - screenStart.Y)
        );

        // Draw semi-transparent fill
        spriteBatch.Draw(pixel, rect, new Color(255, 255, 0, 50));

        // Draw border
        int borderWidth = 2;
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, borderWidth), Color.Yellow); // Top
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Bottom - borderWidth, rect.Width, borderWidth), Color.Yellow); // Bottom
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, borderWidth, rect.Height), Color.Yellow); // Left
        spriteBatch.Draw(pixel, new Rectangle(rect.Right - borderWidth, rect.Y, borderWidth, rect.Height), Color.Yellow); // Right
    }
} 