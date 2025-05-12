using System;
using DefaultEcs;
using Infrastructure.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Camera;
using UI.Factories.Interfaces;
using UI.Rendering.Interfaces;

namespace UI.Rendering;
public class HoverRenderSystem : IRenderSystem
{
    private SpriteFont font;
    private readonly IInputService inputService;
    private readonly Camera2D camera;
    private readonly int tileSize = 32;
    private readonly IFontFactory fontFactory;

    public HoverRenderSystem(IFontFactory fontFactory,
        IInputService inputService,
        Camera2D camera)
    {
        this.camera = camera ?? throw new ArgumentNullException(nameof(camera));
        this.inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
        this.fontFactory = fontFactory;
    }

    public void Draw(SpriteBatch spriteBatch, World world)
    {
        var ms = inputService.GetMousePosition();

        var worldPos = camera.ScreenToWorld(new Vector2(ms.X, ms.Y), spriteBatch.GraphicsDevice);

        int tx = (int)(worldPos.X / tileSize);
        int ty = (int)(worldPos.Y / tileSize);

        var text = $"{tx}, {ty}";
        spriteBatch.DrawString(
            font,
            text,
            new Vector2(ms.X + 8, ms.Y + 8),
            Color.White
        );
    }

    public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        font = fontFactory.GetFont("DefaultFont");
    }
}
