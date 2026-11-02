using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Eycraft.Framework.Objects;

public class Sprite
{
    private readonly Mesh _mesh = new ();

    private Material? _material;

    public Material? Material
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

    private TextureRegion? _region;

    public TextureRegion? Region
    {
        get { return _region; }
        set
        {
            if (!ReferenceEquals (_region, value))
            {
                _region = value;
                AlignOrigin ();
                _dirty = true;
            }
        }
    }

    private Vector2? _preferredSize;

    public Vector2? PreferredSize
    {
        get { return _preferredSize; }
        set
        {
            if (_preferredSize != value)
            {
                _preferredSize = value;
                AlignOrigin ();
                _dirty = true;
            }
        }
    }

    public Vector2 Size
    {
        get
        {
            if (_preferredSize.HasValue)
            {
                return _preferredSize.Value;
            }

            return _region != null ? new Vector2 (_region.Width, _region.Height) : Vector2.Zero;
        }
    }

    private Vector2 _position = Vector2.Zero;

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

    private Color _color = Color.White;

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

    private float _rotation = 0f;

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

    private Vector2 _origin = Vector2.Zero;

    public Vector2 Origin
    {
        get { return _origin; }
        set
        {
            if (_originAlignment != OriginAlignment.None)
            {
                _originAlignment = OriginAlignment.None;
            }

            if (_origin != value)
            {
                _origin = value;
                _dirty = true;
            }
        }
    }

    private OriginAlignment _originAlignment = OriginAlignment.Center;

    public OriginAlignment OriginAlignment
    {
        get { return _originAlignment; }
        set
        {
            if (_originAlignment != value)
            {
                _originAlignment = value;
                AlignOrigin ();
                _dirty = true;
            }
        }
    }

    private SpriteEffects _spriteEffects = SpriteEffects.None;

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

    private float _depth = 0f;

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

    private bool _dirty = true;

    private void AlignOrigin ()
    {
        Vector2 size = Size;
        _origin = AlignOrigin (_originAlignment, size.X, size.Y);
    }

    private Vector2 AlignOrigin (OriginAlignment alignment, float x, float y) => alignment switch
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

    private void PopulateMesh ()
    {
        float x = _position.X;
        float y = _position.Y;

        Vector2 size = Size;
        float w = size.X;
        float h = size.Y;

        float dx = -_origin.X;
        float dy = -_origin.Y;

        if (_rotation == 0f)
        {
            _mesh.SetVertices ([
                new Vector3 (x + dx, y + dy, _depth),
                new Vector3 (x + dx + w, y + dy, _depth),
                new Vector3 (x + dx, y + dy + h, _depth),
                new Vector3 (x + dx + w, y + dy + h, _depth)
                ]);
        }
        else
        {
            float sin = float.Sin (_rotation);
            float cos = float.Cos (_rotation);

            _mesh.SetVertices ([
                new Vector3 (x + dx * cos - dy * sin, y + dx * sin + dy * cos, _depth),
                new Vector3 (x + (dx + w) * cos - dy * sin, y + (dx + w) * sin + dy * cos, _depth),
                new Vector3 (x + dx * cos - (dy + h) * sin, y + dx * sin + (dy + h) * cos, _depth),
                new Vector3 (x + (dx + w) * cos - (dy + h) * sin, y + (dx + w) * sin + (dy + h) * cos, _depth)
                ]);
        }

        _mesh.SetColors ([_color, _color, _color, _color]);

        float top = 0f;
        float bottom = 1f;
        float left = 0f;
        float right = 1f;

        if (_region != null)
        {
            top = _region.TopTextureCoordinate;
            bottom = _region.BottomTextureCoordinate;
            left = _region.LeftTextureCoordinate;
            right = _region.RightTextureCoordinate;
        }

        if (_spriteEffects.HasFlag (SpriteEffects.FlipHorizontally))
        {
            (left, right) = (right, left);
        }

        if (_spriteEffects.HasFlag (SpriteEffects.FlipVertically))
        {
            (top, bottom) = (bottom, top);
        }

        _mesh.SetUVs ([
            new Vector2 (left, top),
            new Vector2 (right, top),
            new Vector2 (left, bottom),
            new Vector2 (right, bottom)
            ]);
    }

    public void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        _material ??= Material.Sprite;

        if (_region != null)
        {
            render.Enqueue (new RenderCommand (_material, null, _mesh, _region.Texture));
        }
    }
}
