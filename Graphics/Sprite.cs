using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public class Sprite (TextureRegion region)
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

    public TextureRegion Region
    {
        get { return _region; }
        set
        {
            if (!ReferenceEquals (_region, value))
            {
                _region = value;
                _dirty = true;
            }
        }
    }

    public Vector2 Size
    {
        get { return _size; }
        set
        {
            if (_size != value)
            {
                _size = value;
                _dirty = true;
            }
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

    public Vector2 Origin
    {
        get { return _origin; }
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

    private readonly Mesh _mesh = new ();

    private TextureRegion _region = region;

    private Vector2 _size = new (region.Width, region.Height);

    private Vector2 _position = Vector2.Zero;

    private Color _color = Color.White;

    private float _rotation = 0f;

    private Vector2 _origin = new (region.Width / 2f, region.Height / 2f);

    private SpriteEffects _spriteEffects = SpriteEffects.None;

    private float _depth = 0f;

    private bool _dirty = true;

    private void PopulateMesh ()
    {
        float x = _position.X;
        float y = _position.Y;
        float w = _size.X;
        float h = _size.Y;
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

        float top = _region.TopTextureCoordinate;
        float bottom = _region.BottomTextureCoordinate;
        float left = _region.LeftTextureCoordinate;
        float right = _region.RightTextureCoordinate;

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

        _material ??= render.SpriteMaterial;

        render.Enqueue (new RenderCommand (_material, null, _mesh, Region.Texture));
    }

    public Sprite Clone () => new (Region)
    {
        Material = Material,
        Size = Size,
        Position = Position,
        Color = Color,
        Rotation = Rotation,
        Origin = Origin,
        SpriteEffects = SpriteEffects,
        Depth = Depth
    };
}
