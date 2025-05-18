using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.Infrastructure.Input;
using CityStateSim.Gameplay.Services.Interfaces;
using CityStateSim.UI;

namespace CityStateSim.UI.Rendering
{
    public class ToolbarButton
    {
        private readonly Texture2D texture;
        private readonly ToolType toolType;
        private readonly IInputService inputService;
        private readonly IToolStateService toolStateService;
        private bool wasMouseDown;
        private Rectangle bounds;

        public ToolbarButton(
            Texture2D texture,
            ToolType toolType,
            IInputService inputService,
            IToolStateService toolStateService)
        {
            this.texture = texture ?? throw new ArgumentNullException(nameof(texture));
            this.toolType = toolType;
            this.inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            this.toolStateService = toolStateService ?? throw new ArgumentNullException(nameof(toolStateService));
        }

        public void SetBounds(int x, int y, int size)
        {
            bounds = new Rectangle(x, y, size, size);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var mousePos = inputService.GetMousePosition();
            bool isHovered = bounds.Contains(mousePos.X, mousePos.Y);
            bool isSelected = toolStateService.CurrentTool == toolType;
            
            Color buttonColor = isSelected ? Color.Yellow : (isHovered ? Color.LightGray : Color.White);
            spriteBatch.Draw(texture, bounds, buttonColor);

            // Handle click with debouncing
            bool isMouseDown = inputService.IsMouseButtonDown(MouseButton.Left);
            if (isHovered && isMouseDown && !wasMouseDown)
            {
                toolStateService.CurrentTool = toolStateService.CurrentTool == toolType ? ToolType.None : toolType;
            }
            wasMouseDown = isMouseDown;
        }
    }
} 