using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.UI.Factories.Interfaces;

namespace CityStateSim.UI.Factories;
public class FontFactory : IFontFactory
{
    private readonly Dictionary<string, SpriteFont> fonts = new();

    public SpriteFont GetFont(string key)
    {
        if (fonts.TryGetValue(key, out var font))
        {
            return font;
        }
        else
        {
            throw new KeyNotFoundException($"Font with key '{key}' not found.");
        }
    }

    public void RegisterFont(string key, SpriteFont font)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));
        }

        if (font == null)
        {
            throw new ArgumentNullException(nameof(font), "Font cannot be null.");
        }

        if (fonts.ContainsKey(key))
        {
            throw new InvalidOperationException($"Font with key '{key}' is already registered.");
        }

        fonts[key] = font;
    }
}
