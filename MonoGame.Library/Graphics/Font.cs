using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Library.Graphics;

public class Font : ResourceRegistry<Font>, IResource, IDisposable
{
    public ushort Id { get; }

    public string Name { get; }

    public SpriteFont SpriteFont { get; }

    public int LineSpacing => SpriteFont.LineSpacing;

    public float Spacing => SpriteFont.Spacing;

    public Dictionary<char, SpriteFont.Glyph> Glyphs { get; }

    public Texture2DResource Texture { get; }

    public Vector2 MeasureString (string text) => SpriteFont.MeasureString (text);

    public Vector2 MeasureString (StringBuilder text) => SpriteFont.MeasureString (text);

    private bool _disposed;

    public Font (string name, SpriteFont spriteFont)
    {
        Id = Regist (name, this);
        Name = name;
        SpriteFont = spriteFont;
        Glyphs = spriteFont.GetGlyphs ();
        Texture = new Texture2DResource (name, spriteFont.Texture);
    }

    ~Font () => Dispose (false);

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
                UnRegist (this);
                Texture.Dispose ();
            }

            _disposed = true;
        }
    }
}
