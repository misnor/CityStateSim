using System;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.UI.Rendering.Interfaces;
using CityStateSim.UI.Factories.Interfaces;

namespace CityStateSim.UI.Rendering
{
    public class ToolbarRenderSystem : IRenderSystem
    {
        private readonly ITextureFactory textureFactory;
        private Texture2D buttonTex;
        private Texture2D bgTex;

        private const int ButtonSize = 48;
        private const int Spacing = 8;
        private const int MarginY = 16;

        public ToolbarRenderSystem(ITextureFactory textureFactory)
        {
            this.textureFactory = textureFactory
                ?? throw new ArgumentNullException(nameof(textureFactory));
        }

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            bgTex = textureFactory.GetTexture("whitePixel");
            buttonTex = textureFactory.GetTexture("tool_axe_single");
        }

        public void Draw(SpriteBatch spriteBatch, World world)
        {
            var vp = spriteBatch.GraphicsDevice.Viewport;
            int buttonCount = 1;
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

            // each button
            for (int i = 0; i < buttonCount; i++)
            {
                int x = startX + i * (ButtonSize + Spacing);
                var dest = new Rectangle(x, posY, ButtonSize, ButtonSize);
                spriteBatch.Draw(buttonTex, dest, Color.White);
            }
        }
    }
}
