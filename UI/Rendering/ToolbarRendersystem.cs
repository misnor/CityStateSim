using System;
using System.Collections.Generic;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.UI.Rendering.Interfaces;
using CityStateSim.UI.Factories.Interfaces;
using CityStateSim.Gameplay.Services.Interfaces;
using CityStateSim.Infrastructure.Input;
using CityStateSim.Core.Commands;
using Microsoft.Extensions.Logging;

namespace CityStateSim.UI.Rendering
{
    public class ToolbarRenderSystem : IRenderSystem
    {
        private readonly ITextureFactory textureFactory;
        private readonly IFontFactory fontFactory;
        private readonly IInputService inputService;
        private readonly IToolStateService toolStateService;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly ITickSpeedService speedService;
        private readonly ILogger<ToolbarRenderSystem> logger;
        private Texture2D bgTex;
        private readonly List<ToolbarButton> buttons;

        private const int ButtonSize = 48;
        private const int Spacing = 8;
        private const int MarginY = 16;

        public ToolbarRenderSystem(
            ITextureFactory textureFactory,
            IFontFactory fontFactory,
            IInputService inputService,
            IToolStateService toolStateService,
            ICommandDispatcher commandDispatcher,
            ITickSpeedService speedService,
            ILogger<ToolbarRenderSystem> logger)
        {
            this.textureFactory = textureFactory ?? throw new ArgumentNullException(nameof(textureFactory));
            this.fontFactory = fontFactory ?? throw new ArgumentNullException(nameof(fontFactory));
            this.inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            this.toolStateService = toolStateService ?? throw new ArgumentNullException(nameof(toolStateService));
            this.commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            this.speedService = speedService ?? throw new ArgumentNullException(nameof(speedService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.buttons = new List<ToolbarButton>();
        }

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            bgTex = textureFactory.GetTexture("whitePixel");
            
            var axeButtonTex = textureFactory.GetTexture("tool_axe_single");
            var cancelButtonTex = textureFactory.GetTexture("cross_large");
            var pickButtonText = textureFactory.GetTexture("tool_pickaxe");

            buttons.Add(new ToolbarButton(axeButtonTex, ToolType.Axe, inputService, toolStateService));
            buttons.Add(new ToolbarButton(pickButtonText, ToolType.Pickaxe, inputService, toolStateService));
            buttons.Add(new ToolbarButton(cancelButtonTex, ToolType.Cancel, inputService, toolStateService));

            logger.LogInformation("ToolbarRenderSystem initialized with {Count} buttons", buttons.Count);
        }

        public void Draw(SpriteBatch spriteBatch, World world)
        {
            var vp = spriteBatch.GraphicsDevice.Viewport;
            int totalWidth = buttons.Count * ButtonSize + (buttons.Count - 1) * Spacing;
            int startX = (vp.Width - totalWidth) / 2;
            int posY = vp.Height - ButtonSize - MarginY;

            var barRect = new Rectangle(
                0,
                vp.Height - ButtonSize - MarginY * 2,
                vp.Width,
                ButtonSize + MarginY * 2 
            );
            spriteBatch.Draw(bgTex, barRect, new Color(0, 0, 0, 150));

            for (int i = 0; i < buttons.Count; i++)
            {
                int x = startX + (i * (ButtonSize + Spacing));
                buttons[i].SetBounds(x, posY, ButtonSize);
                buttons[i].Draw(spriteBatch);
            }

            DrawSpeed(spriteBatch);
        }

        private void DrawSpeed(SpriteBatch spriteBatch)
        {
            var speed = this.speedService.CurrentMultiplier;
            var font = fontFactory.GetFont("DefaultFont");
            var text = $"{speed}x";

            var vp = spriteBatch.GraphicsDevice.Viewport;
            var textSize = font.MeasureString(text);
            var position = new Vector2(vp.Width - textSize.X - 10, vp.Height - textSize.Y - 10);

            spriteBatch.DrawString(font, text, position, Color.White);
        }
    }
}
