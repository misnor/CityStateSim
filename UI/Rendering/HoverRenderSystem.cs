using System;
using System.Collections.Generic;
using System.Linq;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Camera;
using UI.Factories.Interfaces;
using UI.Rendering.Interfaces;
using Infrastructure.Input;
using Core.Components;
using Core.Components.Tags;

namespace UI.Rendering;

public class HoverRenderSystem : IRenderSystem
{
    private SpriteFont font;
    private Texture2D pixel;
    private readonly IInputService inputService;
    private readonly Camera2D camera;
    private readonly IFontFactory fontFactory;
    private readonly ITextureFactory textureFactory;

    private const int TileSize = 32;
    private const int XOffset = 16;
    private const float PaddingX = 4f;
    private const float PaddingY = 2f;
    private const int MaxBoxWidth = 200;

    private static readonly Type[] KnownTypes = new[]
    {
        typeof(PositionComponent),
        typeof(AgentTag),
        typeof(TileTypeComponent),
        // add other component/tag types here
    };

    public HoverRenderSystem(
        IFontFactory fontFactory,
        ITextureFactory textureFactory,
        IInputService inputService,
        Camera2D camera)
    {
        this.fontFactory = fontFactory ?? throw new ArgumentNullException(nameof(fontFactory));
        this.textureFactory = textureFactory ?? throw new ArgumentNullException(nameof(textureFactory));
        this.inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
        this.camera = camera ?? throw new ArgumentNullException(nameof(camera));
    }

    public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        pixel = textureFactory.GetTexture("whitePixel");
        font = fontFactory.GetFont("DefaultFont");
    }

    public void Draw(SpriteBatch spriteBatch, World world)
    {
        var mouseState = inputService.GetMousePosition();
        var screenPos = GetScreenPosition(new Point(mouseState.X, mouseState.Y));
        var worldPos = GetWorldPosition(screenPos, spriteBatch);
        var (tx, ty) = GetTileCoords(worldPos);

        var hovered = FindHoveredEntity(world, tx, ty);
        var lines = BuildLines(hovered, tx, ty);
        var textBlock = string.Join(Environment.NewLine, lines);
        var textSize = MeasureText(textBlock);
        var boxRect = ComputeBackgroundBox(screenPos, textSize);

        DrawBackground(spriteBatch, boxRect);
        DrawText(spriteBatch, textBlock, screenPos);
    }

    private Vector2 GetScreenPosition(Point ms)
        => new Vector2(ms.X, ms.Y);

    private Vector2 GetWorldPosition(Vector2 screenPos, SpriteBatch spriteBatch)
        => camera.ScreenToWorld(screenPos, spriteBatch.GraphicsDevice, false);

    private (int tx, int ty) GetTileCoords(Vector2 worldPos)
        => ((int)(worldPos.X / TileSize), (int)(worldPos.Y / TileSize));

    private Entity FindHoveredEntity(World world, int tx, int ty)
    {
        var hits = world
          .GetEntities()
          .With<PositionComponent>()
          .AsEnumerable()
          .Where(e =>
          {
              ref var p = ref e.Get<PositionComponent>();
              return p.X == tx && p.Y == ty;
          });

        return hits
          .OrderByDescending(e => e.Has<AgentTag>() ? 1 : 0)
          .FirstOrDefault();
    }

    private List<string> BuildLines(Entity hovered, int tx, int ty)
    {
        var lines = new List<string> { $"{tx}, {ty}" };
        if (hovered.IsAlive)
        {
            foreach (var type in KnownTypes)
            {
                var hasMethod = typeof(Entity)
                    .GetMethod(nameof(Entity.Has))!
                    .MakeGenericMethod(type);

                bool has = (bool)hasMethod.Invoke(hovered, null)!;
                if (has)
                {
                    lines.Add(type.Name);
                }
            }
        }
        return lines;
    }

    private Vector2 MeasureText(string text)
        => font.MeasureString(text);

    private Rectangle ComputeBackgroundBox(Vector2 screenPos, Vector2 textSize)
    {
        float boxWidth = Math.Min(textSize.X + PaddingX * 2, MaxBoxWidth);
        float boxHeight = textSize.Y + PaddingY * 2;

        var pos = new Vector2(
            screenPos.X + XOffset - PaddingX,
            screenPos.Y + PaddingY
        );

        return new Rectangle(
            (int)pos.X,
            (int)pos.Y,
            (int)boxWidth,
            (int)boxHeight
        );
    }

    private void DrawBackground(SpriteBatch sb, Rectangle boxRect)
        => sb.Draw(pixel, boxRect, new Color(0, 0, 0, 150));

    private void DrawText(SpriteBatch sb, string text, Vector2 screenPos)
        => sb.DrawString(
            font,
            text,
            screenPos + new Vector2(XOffset, PaddingY),
            Color.White
        );
}
