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

    public Vector2 Scale
    {
        get { return _scale; }
        set
        {
            if (_scale != value)
            {
                _scale = value;
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

    public float LayerDepth
    {
        get { return _layerDepth; }
        set
        {
            if (_layerDepth != value)
            {
                _layerDepth = value;
                _dirty = true;
            }
        }
    }

    public float Width => Region.Width * Scale.X;

    public float Height => Region.Height * Scale.Y;

    private MaterialInstance? _material;

    private readonly Mesh _mesh = new ();

    private TextureRegion _region = region;

    private Vector2 _position = Vector2.Zero;

    private Color _color = Color.White;

    private float _rotation = 0f;

    private Vector2 _scale = Vector2.One;

    private Vector2 _origin = Vector2.Zero;

    private SpriteEffects _spriteEffects = SpriteEffects.None;

    private float _layerDepth = 0f;

    private bool _dirty = true;

    private void PopulateMesh ()
    {
        float x = Position.X;
        float y = Position.Y;
        float w = Width;
        float h = Height;
        float depth = LayerDepth;

        _mesh.SetVertices ([
            new Vector3 (x, y, depth),
            new Vector3 (x + w, y, depth),
            new Vector3 (x, y + h, depth),
            new Vector3 (x + w, y + h, depth)
            ]);

        _mesh.SetColors ([Color, Color, Color, Color]);

        float top = Region.TopTextureCoordinate;
        float bottom = Region.BottomTextureCoordinate;
        float left = Region.LeftTextureCoordinate;
        float right = Region.RightTextureCoordinate;

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
