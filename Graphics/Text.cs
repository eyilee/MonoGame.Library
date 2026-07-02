using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Library.Graphics;

public class Text (FontResource font)
{
    public MaterialInstance? Material
    {
        get { return _material; }
        set
        {
            if (!ReferenceEquals (_material, value))
            {
                _material = value;
                _dirty = true;
            }
        }
    }

    public FontResource Font
    {
        get { return _font; }
        set
        {
            if (!ReferenceEquals (_font, value))
            {
                _font = value;
                _dirty = true;
                _sizeDirty = true;
            }
        }
    }

    public string Value
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                _value = value;
                _dirty = true;
                _sizeDirty = true;
            }
        }
    }

    public Vector2 Size
    {
        get
        {
            if (_sizeDirty)
            {
                _size = _font.MeasureString (_value);
                _sizeDirty = false;
            }

            return _size;
        }
    }

    public Vector2 Position
    {
        get { return _position; }
        set
        {
            if (_position != value)
            {
                _position = value;
                _dirty = true;
            }
        }
    }

    public Color Color
    {
        get { return _color; }
        set
        {
            if (_color != value)
            {
                _color = value;
                _dirty = true;
            }
        }
    }

    public float Rotation
    {
        get { return _rotation; }
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                _dirty = true;
            }
        }
    }

    public Vector2? Origin
    {
        get { return _origin ?? Vector2.Zero; }
        set
        {
            if (_origin != value)
            {
                _origin = value;
                _dirty = true;
            }
        }
    }

    public SpriteEffects SpriteEffects
    {
        get { return _spriteEffects; }
        set
        {
            if (_spriteEffects != value)
            {
                _spriteEffects = value;
                _dirty = true;
            }
        }
    }

    public float Depth
    {
        get { return _depth; }
        set
        {
            if (_depth != value)
            {
                _depth = value;
                _dirty = true;
            }
        }
    }

    private MaterialInstance? _material;

    private readonly List<Mesh> _meshes = [];

    private FontResource _font = font;

    private string _value = string.Empty;

    private Vector2 _size = Vector2.Zero;

    private Vector2 _position = Vector2.Zero;

    private Color _color = Color.White;

    private float _rotation = 0f;

    private Vector2? _origin;

    private SpriteEffects _spriteEffects = SpriteEffects.None;

    private float _depth = 0f;

    private bool _dirty = true;

    private bool _sizeDirty = true;

    private void PopulateMesh ()
    {
        _meshes.Clear ();

        if (string.IsNullOrEmpty (_value))
        {
            return;
        }

        Vector2 origin = _origin ?? _size / 2f;
        bool flipHorizontally = _spriteEffects.HasFlag (SpriteEffects.FlipHorizontally);
        bool flipVertically = _spriteEffects.HasFlag (SpriteEffects.FlipVertically);

        Matrix transform = Matrix.CreateTranslation (-_position.X - origin.X, -_position.Y - origin.Y, 0f)
            * Matrix.CreateScale (flipHorizontally ? -1f : 1f, flipVertically ? -1f : 1f, 1f)
            * Matrix.CreateRotationZ (_rotation)
            * Matrix.CreateTranslation (_position.X, _position.Y, 0f);

        Vector2 offset = Vector2.Zero;
        bool firstGlyphOfLine = true;

        for (var i = 0; i < _value.Length; ++i)
        {
            char c = _value[i];

            if (c == '\r')
            {
                continue;
            }

            if (c == '\n')
            {
                offset.X = 0;
                offset.Y += _font.LineSpacing;
                firstGlyphOfLine = true;
                continue;
            }

            if (!_font.Glyphs.TryGetValue (c, out SpriteFont.Glyph glyph))
            {
                continue;
            }

            if (firstGlyphOfLine)
            {
                offset.X = float.Max (glyph.LeftSideBearing, 0);
                firstGlyphOfLine = false;
            }
            else
            {
                offset.X += _font.Spacing + glyph.LeftSideBearing;
            }

            float x = _position.X + offset.X + glyph.Cropping.X;
            float y = _position.Y + offset.Y + glyph.Cropping.Y;
            float w = glyph.BoundsInTexture.Width;
            float h = glyph.BoundsInTexture.Height;

            if (flipHorizontally)
            {
                x += w;
                w = -w;
            }

            if (flipVertically)
            {
                y += h;
                h = -h;
            }

            float top = glyph.BoundsInTexture.Y * _font.Texture.TexelHeight;
            float bottom = (glyph.BoundsInTexture.Y + glyph.BoundsInTexture.Height) * _font.Texture.TexelHeight;
            float left = glyph.BoundsInTexture.X * _font.Texture.TexelWidth;
            float right = (glyph.BoundsInTexture.X + glyph.BoundsInTexture.Width) * _font.Texture.TexelWidth;

            if (flipHorizontally)
            {
                (left, right) = (right, left);
            }

            if (flipVertically)
            {
                (top, bottom) = (bottom, top);
            }

            Mesh mesh = new ();

            mesh.SetVertices ([
                Vector3.Transform (new Vector3 (x, y, _depth), transform),
                Vector3.Transform (new Vector3 (x + w, y, _depth), transform),
                Vector3.Transform (new Vector3 (x, y + h, _depth), transform),
                Vector3.Transform (new Vector3 (x + w, y + h, _depth), transform)
                ]);

            mesh.SetColors ([Color, Color, Color, Color]);

            mesh.SetUVs ([
                new Vector2 (left, top),
                new Vector2 (right, top),
                new Vector2 (left, bottom),
                new Vector2 (right, bottom)
                ]);

            _meshes.Add (mesh);

            offset.X += glyph.Width + glyph.RightSideBearing;
        }
    }

    public void Draw (RenderManager render)
    {
        if (_sizeDirty)
        {
            _size = _font.MeasureString (_value);
            _sizeDirty = false;
        }

        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        _material ??= render.SpriteMaterial;

        foreach (Mesh mesh in _meshes)
        {
            render.Enqueue (new RenderCommand (_material, null, mesh, _font.Texture));
        }
    }

    public Text Clone () => new (_font)
    {
        Material = _material,
        Value = _value,
        Position = _position,
        Color = _color,
        Rotation = _rotation,
        Origin = _origin,
        SpriteEffects = _spriteEffects,
        Depth = _depth
    };
}
