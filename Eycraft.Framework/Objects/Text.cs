using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Eycraft.Framework.Objects;

public class Text (Entity entity) : EntityComponent (entity)
{
    private readonly Mesh _mesh = new ();

    public Material? Material { get; set; }

    private Font? _font;

    public Font? Font
    {
        get => _font;
        set
        {
            if (!ReferenceEquals (_font, value))
            {
                _font = value;
                _dirty = true;
            }
        }
    }

    private string _value = string.Empty;

    public string Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                _dirty = true;
            }
        }
    }

    private Vector2 _offset = Vector2.Zero;

    public Vector2 Offset
    {
        get => _offset;
        set
        {
            if (_offset != value)
            {
                _offset = value;
                _dirty = true;
            }
        }
    }

    private Color _color = Color.White;

    public Color Color
    {
        get => _color;
        set
        {
            if (_color != value)
            {
                _color = value;
                _dirty = true;
            }
        }
    }

    private float _rotation = 0f;

    public float Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                _dirty = true;
            }
        }
    }

    private Vector2 _origin = Vector2.Zero;

    private OriginAlignment _originAlignment = OriginAlignment.TopLeft;

    public OriginAlignment OriginAlignment
    {
        get => _originAlignment;
        set
        {
            if (_originAlignment != value)
            {
                _originAlignment = value;
                _dirty = true;
            }
        }
    }

    private SpriteEffects _spriteEffects = SpriteEffects.None;

    public SpriteEffects SpriteEffects
    {
        get => _spriteEffects;
        set
        {
            if (_spriteEffects != value)
            {
                _spriteEffects = value;
                _dirty = true;
            }
        }
    }

    private float _depth = 0f;

    public float Depth
    {
        get => _depth;
        set
        {
            if (_depth != value)
            {
                _depth = value;
                _dirty = true;
            }
        }
    }

    public short SortingLayer { get; set; } = 0;

    public short SortingOrder { get; set; } = 0;

    private bool _dirty = true;

    public override void Awake ()
    {
        Transform.OnTransformChanged += OnTransformChanged;
    }

    public override void Destroy ()
    {
        Transform.OnTransformChanged -= OnTransformChanged;
    }

    protected virtual void OnTransformChanged (Transform.ChangedFlags changedFlags)
    {
        _dirty = true;
    }

    private void AlignOrigin (Vector2 size)
    {
        float x = size.X;
        float y = size.Y;

        _origin = OriginAlignment switch
        {
            OriginAlignment.None => _origin,
            OriginAlignment.TopLeft => new Vector2 (0f, 0f),
            OriginAlignment.TopCenter => new Vector2 (x * 0.5f, 0f),
            OriginAlignment.TopRight => new Vector2 (x, 0f),
            OriginAlignment.CenterLeft => new Vector2 (0f, y * 0.5f),
            OriginAlignment.Center => new Vector2 (x * 0.5f, y * 0.5f),
            OriginAlignment.CenterRight => new Vector2 (x, y * 0.5f),
            OriginAlignment.BottomLeft => new Vector2 (0f, y),
            OriginAlignment.BottomCenter => new Vector2 (x * 0.5f, y),
            OriginAlignment.BottomRight => new Vector2 (x, y),
            _ => _origin
        };
    }

    private void PopulateMesh ()
    {
        if (Font == null || string.IsNullOrEmpty (Value))
        {
            return;
        }

        Vector2 size = Font.MeasureString (Value);
        AlignOrigin (size);

        bool flipHorizontally = SpriteEffects.HasFlag (SpriteEffects.FlipHorizontally);
        bool flipVertically = SpriteEffects.HasFlag (SpriteEffects.FlipVertically);

        Matrix transform = Matrix.CreateTranslation (-_origin.X, -_origin.Y, 0f)
            * Matrix.CreateScale (flipHorizontally ? -1f : 1f, flipVertically ? -1f : 1f, 1f)
            * Matrix.CreateRotationZ (Rotation)
            * Matrix.CreateTranslation (Offset.X, Offset.Y, 0f)
            * Matrix.CreateRotationZ (Transform.Rotation)
            * Matrix.CreateTranslation (Transform.Position.X, Transform.Position.Y, 0f);

        Vector2 offset = Vector2.Zero;
        bool firstGlyphOfLine = true;

        List<ushort> indices = new (Value.Length * 6);
        List<Vector3> vertices = new (Value.Length * 4);
        List<Color> colors = new (Value.Length * 4);
        List<Vector2> uvs = new (Value.Length * 4);

        for (var i = 0; i < Value.Length; ++i)
        {
            char c = Value[i];

            if (c == '\r')
            {
                continue;
            }

            if (c == '\n')
            {
                offset.X = 0;
                offset.Y += Font.LineSpacing;
                firstGlyphOfLine = true;
                continue;
            }

            if (!Font.Glyphs.TryGetValue (c, out SpriteFont.Glyph glyph))
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
                offset.X += Font.Spacing + glyph.LeftSideBearing;
            }

            float x = offset.X + glyph.Cropping.X;
            float y = offset.Y + glyph.Cropping.Y;
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

            float top = glyph.BoundsInTexture.Y * Font.Texture.TexelHeight;
            float bottom = (glyph.BoundsInTexture.Y + glyph.BoundsInTexture.Height) * Font.Texture.TexelHeight;
            float left = glyph.BoundsInTexture.X * Font.Texture.TexelWidth;
            float right = (glyph.BoundsInTexture.X + glyph.BoundsInTexture.Width) * Font.Texture.TexelWidth;

            if (flipHorizontally)
            {
                (left, right) = (right, left);
            }

            if (flipVertically)
            {
                (top, bottom) = (bottom, top);
            }

            ushort index = (ushort)(i * 4);
            indices.Add ((ushort)(index + 0));
            indices.Add ((ushort)(index + 1));
            indices.Add ((ushort)(index + 2));
            indices.Add ((ushort)(index + 1));
            indices.Add ((ushort)(index + 3));
            indices.Add ((ushort)(index + 2));

            vertices.Add (Vector3.Transform (new Vector3 (x, y, Depth), transform));
            vertices.Add (Vector3.Transform (new Vector3 (x + w, y, Depth), transform));
            vertices.Add (Vector3.Transform (new Vector3 (x, y + h, Depth), transform));
            vertices.Add (Vector3.Transform (new Vector3 (x + w, y + h, Depth), transform));

            colors.Add (Color.White);
            colors.Add (Color.White);
            colors.Add (Color.White);
            colors.Add (Color.White);

            uvs.Add (new Vector2 (left, top));
            uvs.Add (new Vector2 (right, top));
            uvs.Add (new Vector2 (left, bottom));
            uvs.Add (new Vector2 (right, bottom));

            offset.X += glyph.Width + glyph.RightSideBearing;
        }

        _mesh.SetIndices ([.. indices]);
        _mesh.SetVertices ([.. vertices]);
        _mesh.SetColors ([.. colors]);
        _mesh.SetUVs ([.. uvs]);
    }

    public void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        Material ??= Material.SpriteFont;

        if (Font != null)
        {
            render.Enqueue (RenderCommand.CreateSingleTexture (Material, _mesh, Font.Texture, SortingLayer, SortingOrder));
        }
    }
}
