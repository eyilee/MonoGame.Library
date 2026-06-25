using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public class Sprite (TextureRegion region)
{
    public TextureRegion Region
    {
        get { return _region; }
        set
        {
            _region = value;
            _dirty = true;
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
        float depth = _depth;

        if (_rotation == 0f)
        {
            _mesh.SetVertices ([
                new Vector3 (x - dx, y - dy, depth),
                new Vector3 (x - dx + w, y - dy, depth),
                new Vector3 (x - dx, y - dy + h, depth),
                new Vector3 (x - dx + w, y - dy + h, depth)
                ]);
        }
        else
        {
            float sin = float.Sin (_rotation);
            float cos = float.Cos (_rotation);

            _mesh.SetVertices ([
                new Vector3 (x + dx * cos - dy * sin, y + dx * sin + dy * cos, depth),
                new Vector3 (x + (dx + w) * cos - dy * sin, y + (dx + w) * sin + dy * cos, depth),
                new Vector3 (x + dx * cos - (dy + h) * sin, y + dx * sin + (dy + h) * cos, depth),
                new Vector3 (x + (dx + w) * cos - (dy + h) * sin, y + (dx + w) * sin + (dy + h) * cos, depth)
                ]);
        }

        _mesh.SetColors ([Color, Color, Color, Color]);

        float top = _region.TopTextureCoordinate;
        float bottom = _region.BottomTextureCoordinate;
        float left = _region.LeftTextureCoordinate;
        float right = _region.RightTextureCoordinate;

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
}
