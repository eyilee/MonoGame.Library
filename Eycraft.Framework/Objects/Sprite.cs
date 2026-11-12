using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Eycraft.Framework.Objects;

public class Sprite (Entity entity) : EntityComponent (entity)
{
    private readonly Mesh _mesh = new ();

    public Material? Material { get; set; }

    private TextureRegion? _region;

    public TextureRegion? Region
    {
        get => _region;
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

    private Vector2? _preferredSize;

    public Vector2? PreferredSize
    {
        get => _preferredSize;
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
            if (PreferredSize.HasValue)
            {
                return PreferredSize.Value;
            }

            return Region != null ? new Vector2 (Region.Width, Region.Height) : Vector2.Zero;
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

    public Vector2 Origin
    {
        get => _origin;
        set
        {
            OriginAlignment = OriginAlignment.None;

            if (_origin != value)
            {
                _origin = value;
                _dirty = true;
            }
        }
    }

    private OriginAlignment _originAlignment = OriginAlignment.None;

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

    private void AlignOrigin ()
    {
        Vector2 size = Size;
        float x = size.X;
        float y = size.Y;

        Origin = OriginAlignment switch
        {
            OriginAlignment.None => Origin,
            OriginAlignment.TopLeft => new Vector2 (0f, 0f),
            OriginAlignment.TopCenter => new Vector2 (x * 0.5f, 0f),
            OriginAlignment.TopRight => new Vector2 (x, 0f),
            OriginAlignment.CenterLeft => new Vector2 (0f, y * 0.5f),
            OriginAlignment.Center => new Vector2 (x * 0.5f, y * 0.5f),
            OriginAlignment.CenterRight => new Vector2 (x, y * 0.5f),
            OriginAlignment.BottomLeft => new Vector2 (0f, y),
            OriginAlignment.BottomCenter => new Vector2 (x * 0.5f, y),
            OriginAlignment.BottomRight => new Vector2 (x, y),
            _ => Origin
        };
    }

    private void PopulateMesh ()
    {
        Vector2 size = Size;
        float w = size.X;
        float h = size.Y;

        bool flipHorizontally = SpriteEffects.HasFlag (SpriteEffects.FlipHorizontally);
        bool flipVertically = SpriteEffects.HasFlag (SpriteEffects.FlipVertically);

        Matrix transform = Matrix.CreateTranslation (-Origin.X, -Origin.Y, 0f)
            * Matrix.CreateScale (flipHorizontally ? -1f : 1f, flipVertically ? -1f : 1f, 1f)
            * Matrix.CreateRotationZ (Rotation)
            * Matrix.CreateTranslation (Offset.X, Offset.Y, 0f)
            * Matrix.CreateRotationZ (Transform.Rotation)
            * Matrix.CreateTranslation (Transform.Position.X, Transform.Position.Y, 0f);

        if (flipHorizontally)
        {
            w = -w;
        }

        if (flipVertically)
        {
            h = -h;
        }

        _mesh.SetVertices ([
            Vector3.Transform (new Vector3 (0f, 0f, Depth), transform),
            Vector3.Transform (new Vector3 (w, 0f, Depth), transform),
            Vector3.Transform (new Vector3 (0f, h, Depth), transform),
            Vector3.Transform (new Vector3 (w, h, Depth), transform)
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

        if (flipHorizontally)
        {
            (left, right) = (right, left);
        }

        if (flipVertically)
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

        Material ??= Material.Sprite;

        if (Region != null)
        {
            render.Enqueue (RenderCommand.CreateSingleTexture (Material, _mesh, Region.Texture, Depth));
        }
    }
}
