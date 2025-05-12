using Microsoft.Xna.Framework.Graphics;

namespace CityStateSim.UI.Factories.Interfaces;
public interface IFontFactory
{
    SpriteFont GetFont(string key);
    void RegisterFont(string key, SpriteFont font);
}
