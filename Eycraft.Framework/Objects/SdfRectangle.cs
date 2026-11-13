using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Objects;

public class SdfRectangle (Entity entity) : SdfShape (entity)
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

    private Vector2 _size = Vector2.Zero;

    public Vector2 Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                _dirty = true;
            }
        }
    }

    public bool Filled { get; set; } = false;

    protected override void PopulateMesh ()
    {
        Vector2 position = Transform.Position + Vector2.Rotate (Offset, Transform.Rotation);
        float rotation = Transform.Rotation + Rotation;
        Vector2 scale = new ((Size.X * 0.5f + Thickness) * 2f, (Size.Y * 0.5f + Thickness) * 2f);

        _mesh.SetUVs ([new Vector3 (position, Depth)]);
        _mesh.SetUV1s ([new Vector4 (rotation, scale.X, scale.Y, Thickness)]);
        _mesh.SetUV2s ([Size * 0.5f]);
        _mesh.SetColors ([Color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (RenderCommand.CreateSingleTexture (Filled ? Material.SdfFilledRectangle : Material.SdfRectangle, _mesh, SortingLayer, SortingOrder));
    }
}
