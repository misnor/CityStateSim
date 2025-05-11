using DefaultEcs;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Rendering.Interfaces;
public interface IRenderSystem
{
    void Draw(SpriteBatch spriteBatch, World world);
}
