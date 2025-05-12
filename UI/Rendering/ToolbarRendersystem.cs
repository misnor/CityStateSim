using System;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.UI.Rendering.Interfaces;
using CityStateSim.UI.Factories.Interfaces;
using CityStateSim.Gameplay.Services.Interfaces;
using CityStateSim.Infrastructure.Input;
using CityStateSim.UI;
using CityStateSim.Core.Commands;
using CityStateSim.Gameplay.Commands;
using Microsoft.Extensions.Logging;

namespace CityStateSim.UI.Rendering
{
    public class ToolbarRenderSystem : IRenderSystem
    {
        private readonly ITextureFactory textureFactory;
        private readonly IInputService inputService;
        private readonly IToolStateService toolStateService;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly ILogger<ToolbarRenderSystem> logger;
        private Texture2D axeButtonTex;
        private Texture2D cancelButtonTex;
        private Texture2D bgTex;
        private bool axeWasMouseDown;
        private bool cancelWasMouseDown;

        private const int ButtonSize = 48;
        private const int Spacing = 8;
        private const int MarginY = 16;

        public ToolbarRenderSystem(
            ITextureFactory textureFactory,
            IInputService inputService,
            IToolStateService toolStateService,
            ICommandDispatcher commandDispatcher,
            ILogger<ToolbarRenderSystem> logger)
        {
            this.textureFactory = textureFactory ?? throw new ArgumentNullException(nameof(textureFactory));
            this.inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            this.toolStateService = toolStateService ?? throw new ArgumentNullException(nameof(toolStateService));
            this.commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            bgTex = textureFactory.GetTexture("whitePixel");
            axeButtonTex = textureFactory.GetTexture("tool_axe_single");
            cancelButtonTex = textureFactory.GetTexture("cross_large");
            logger.LogInformation("ToolbarRenderSystem initialized with textures");
        }

        public void Draw(SpriteBatch spriteBatch, World world)
        {
            var vp = spriteBatch.GraphicsDevice.Viewport;
            int buttonCount = 2; // Now we have 2 buttons
            int totalWidth = buttonCount * ButtonSize + (buttonCount - 1) * Spacing;
            int startX = (vp.Width - totalWidth) / 2;
            int posY = vp.Height - ButtonSize - MarginY;

            // background bar
            var barRect = new Rectangle(
                0,
                vp.Height - ButtonSize - MarginY * 2,
                vp.Width,
                ButtonSize + MarginY * 2
            );
            spriteBatch.Draw(bgTex, barRect, new Color(0, 0, 0, 150));

            // Draw axe button
            DrawButton(spriteBatch, startX, posY, axeButtonTex, ToolType.Axe, ref axeWasMouseDown);

            // Draw cancel button
            DrawButton(spriteBatch, startX + ButtonSize + Spacing, posY, cancelButtonTex, ToolType.Cancel, ref cancelWasMouseDown);
        }

        private void DrawButton(SpriteBatch spriteBatch, int x, int y, Texture2D texture, ToolType toolType, ref bool wasMouseDown)
        {
            var dest = new Rectangle(x, y, ButtonSize, ButtonSize);
            
            // Check if mouse is over this button
            var mousePos = inputService.GetMousePosition();
            bool isHovered = dest.Contains(mousePos.X, mousePos.Y);
            
            // Check if this tool is selected
            bool isSelected = toolStateService.CurrentTool == toolType;
            
            // Draw button with appropriate color
            Color buttonColor = isSelected ? Color.Yellow : (isHovered ? Color.LightGray : Color.White);
            spriteBatch.Draw(texture, dest, buttonColor);

            // Log button state
            logger.LogDebug("Button {ToolType} at ({X}, {Y}): Hovered={IsHovered}, Selected={IsSelected}, Mouse=({MouseX}, {MouseY})",
                toolType, x, y, isHovered, isSelected, mousePos.X, mousePos.Y);

            // Handle click with debouncing
            bool isMouseDown = inputService.IsMouseButtonDown(MouseButton.Left);
            if (isHovered && isMouseDown && !wasMouseDown)
            {
                var oldTool = toolStateService.CurrentTool;
                toolStateService.CurrentTool = toolStateService.CurrentTool == toolType ? ToolType.None : toolType;
                logger.LogInformation("Tool changed from {OldTool} to {NewTool} (clicked {ToolType} button at {X}, {Y})",
                    oldTool, toolStateService.CurrentTool, toolType, x, y);
            }
            wasMouseDown = isMouseDown;
        }
    }
}
