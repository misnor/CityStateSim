using DefaultEcs;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Rendering.Interfaces;
public interface IRenderSystem
{
    void Draw(SpriteBatch spriteBatch, World world);
    void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager);
}
