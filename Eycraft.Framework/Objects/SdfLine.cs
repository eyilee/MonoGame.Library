using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Objects;

public class SdfLine (Entity entity) : SdfShape (entity)
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

    private Vector2 _start = Vector2.Zero;

    public Vector2 Start
    {
        get => _start;
        set
        {
            if (_start != value)
            {
                _start = value;
                _dirty = true;
            }
        }
    }

    private Vector2 _end = Vector2.Zero;

    public Vector2 End
    {
        get => _end;
        set
        {
            if (_end != value)
            {
                _end = value;
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

    protected override void PopulateMesh ()
    {
        Vector2 start = Transform.Position + Vector2.Rotate (Offset + Start, Transform.Rotation);
        Vector2 end = Transform.Position + Vector2.Rotate (Offset + End, Transform.Rotation);
        Vector2 position = (start + end) * 0.5f;
        Vector2 scale = new (float.Abs (end.X - start.X) + Thickness * 2f, float.Abs (end.Y - start.Y) + Thickness * 2f);

        _mesh.SetUVs ([new Vector3 (position, Depth)]);
        _mesh.SetUV1s ([new Vector4 (Rotation, scale.X, scale.Y, Thickness)]);
        _mesh.SetUV2s ([new Vector4 (start.X - position.X, start.Y - position.Y, end.X - position.X, end.Y - position.Y)]);
        _mesh.SetColors ([Color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (RenderCommand.CreateSingleTexture (Material.SdfLine, _mesh, Depth));
    }
}
