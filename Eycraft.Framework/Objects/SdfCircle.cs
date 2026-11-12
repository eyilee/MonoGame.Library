using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Objects;

public class SdfCircle (Entity entity) : SdfShape (entity)
{
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

    private float _radius = 0f;

    public float Radius
    {
        get => _radius;
        set
        {
            if (_radius != value)
            {
                _radius = value;
                _dirty = true;
            }
        }
    }

    public bool Filled { get; set; } = false;

    protected override void PopulateMesh ()
    {
        Vector2 position = Transform.Position + Vector2.Rotate (Offset, Transform.Rotation);
        float rotation = Transform.Rotation + Rotation;
        Vector2 scale = new ((Radius + Thickness) * 2f, (Radius + Thickness) * 2f);

        _mesh.SetUVs ([new Vector3 (position, Depth)]);
        _mesh.SetUV1s ([new Vector4 (rotation, scale.X, scale.Y, Thickness)]);
        _mesh.SetUV2s ([Radius]);
        _mesh.SetColors ([Color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (RenderCommand.CreateSingleTexture (Filled ? Material.SdfFilledCircle : Material.SdfCircle, _mesh, Depth));
    }
}
