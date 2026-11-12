using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Tilemap;

public class TileSprite
{
    private readonly Mesh _mesh = new ();

    private TextureRegion? _region;

    public TextureRegion? Region
    {
        get => _region;
        set
        {
            if (!ReferenceEquals (_region, value))
            {
                _region = value;
                _dirty = true;
            }
        }
    }

    private Vector2 _position;

    public Vector2 Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                _dirty = true;
            }
        }
    }

    private Vector2 _size;

    public Vector2 Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                AlignOrigin ();
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

    private Vector2 _origin = Vector2.Zero;

    private OriginAlignment _originAlignment = OriginAlignment.Center;

    public OriginAlignment OriginAlignment
    {
        get => _originAlignment;
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

    private bool _dirty = true;

    public TileSprite ()
    {
        AlignOrigin ();
    }

    private void AlignOrigin ()
    {
        float x = Size.X;
        float y = Size.Y;

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
        float x = Position.X;
        float y = Position.Y;
        float w = Size.X;
        float h = Size.Y;
        float dx = -_origin.X;
        float dy = -_origin.Y;

        _mesh.SetVertices ([
            new Vector3 (x + dx, y + dy, Depth),
            new Vector3 (x + dx + w, y + dy, Depth),
            new Vector3 (x + dx, y + dy + h, Depth),
            new Vector3 (x + dx + w, y + dy + h, Depth)
            ]);

        _mesh.SetColors ([Color, Color, Color, Color]);

        float top = 0f;
        float bottom = 1f;
        float left = 0f;
        float right = 1f;

        if (Region != null)
        {
            top = Region.TopTextureCoordinate;
            bottom = Region.BottomTextureCoordinate;
            left = Region.LeftTextureCoordinate;
            right = Region.RightTextureCoordinate;
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

        if (Region != null)
        {
            render.Enqueue (RenderCommand.CreateSingleTexture (Material.Sprite, _mesh, Region.Texture, Depth));
        }
    }
}
