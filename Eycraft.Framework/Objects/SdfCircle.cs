using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Objects;

public class SdfCircle (Entity entity) : SdfShape (entity)
{
    protected Vector2 _offset = Vector2.Zero;

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

    protected float _rotation = 0f;

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

    protected float _radius = 0f;

    public float Radius
    {
        get { return _radius; }
        set
        {
            if (_radius != value)
            {
                _radius = value;
                _dirty = true;
            }
        }
    }

    protected bool _filled = false;

    public bool Filled
    {
        get => _filled;
        set
        {
            if (_filled != value)
            {
                _filled = value;
            }
        }
    }

    protected override void PopulateMesh ()
    {
        Vector2 position = Transform.Position + Vector2.Rotate (_offset, Transform.Rotation);
        float rotation = Transform.Rotation + _rotation;
        Vector2 scale = new ((_radius + _thickness) * 2f, (_radius + _thickness) * 2f);

        _mesh.SetUVs ([new Vector3 (position, _depth)]);
        _mesh.SetUV1s ([new Vector4 (rotation, scale.X, scale.Y, _thickness)]);
        _mesh.SetUV2s ([_radius]);
        _mesh.SetColors ([_color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (RenderCommand.CreateSingleTexture (Filled ? Material.SdfFilledCircle : Material.SdfCircle, _mesh, _depth));
    }
}
