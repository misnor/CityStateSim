using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.UI.Factories.Interfaces;

namespace CityStateSim.UI.Factories;
internal class TextureFactory : ITextureFactory
{
    private readonly Dictionary<string, Texture2D> textures = new();

    public Texture2D GetTexture(string key)
    {
        if (textures.TryGetValue(key, out var texture))
        {
            return texture;
        }
        else
        {
            throw new KeyNotFoundException($"Texture with key '{key}' not found.");
        }
    }

    public void RegisterTexture(string key, Texture2D texture)
    {
        textures[key] = texture;
    }
}
