using DefaultEcs;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Rendering.Interfaces;
public interface IRenderService
{
    void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager);
    void Draw(SpriteBatch spriteBatch, World world);
}
