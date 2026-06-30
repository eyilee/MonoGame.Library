using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Library.Graphics;

public class FontResource : INamedResource, IDisposable
{
    public ushort Id { get; }

    public string Name { get; }

    public SpriteFont Font { get; }

    public int LineSpacing => Font.LineSpacing;

    public float Spacing => Font.Spacing;

    public Dictionary<char, SpriteFont.Glyph> Glyphs { get; }

    public Texture2DResource Texture { get; }

    public Vector2 MeasureString (string text) => Font.MeasureString (text);

    public Vector2 MeasureString (StringBuilder text) => Font.MeasureString (text);

    private static readonly ResourceRegistry<FontResource> s_registry = new ();

    private bool _disposed;

    public FontResource (string name, SpriteFont font)
    {
        Id = s_registry.Regist (name, this);
        Name = name;
        Font = font;
        Glyphs = font.GetGlyphs ();
        Texture = new Texture2DResource (name, font.Texture);
    }

    ~FontResource () => Dispose (false);

    public void Dispose ()
    {
        Dispose (true);
        GC.SuppressFinalize (this);
    }

    protected virtual void Dispose (bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                s_registry.UnRegist (this);
                Texture.Dispose ();
            }

            _disposed = true;
        }
    }
}
