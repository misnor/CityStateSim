using Microsoft.Xna.Framework.Graphics;

namespace CityStateSim.UI.Factories.Interfaces;
public interface ITextureFactory
{
    void RegisterTexture(string key, Texture2D texture);
    Texture2D GetTexture(string key);
}
