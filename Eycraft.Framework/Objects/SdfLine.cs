using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Objects;

public class SdfLine (Entity entity) : SdfShape (entity)
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

    protected Vector2 _start = Vector2.Zero;

    public Vector2 Start
    {
        get { return _start; }
        set
        {
            if (_start != value)
            {
                _start = value;
                _dirty = true;
            }
        }
    }

    protected Vector2 _end = Vector2.Zero;

    public Vector2 End
    {
        get { return _end; }
        set
        {
            if (_end != value)
            {
                _end = value;
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

    protected override void PopulateMesh ()
    {
        Vector2 start = Transform.Position + Vector2.Rotate (_offset + _start, Transform.Rotation);
        Vector2 end = Transform.Position + Vector2.Rotate (_offset + _end, Transform.Rotation);
        Vector2 position = (start + end) * 0.5f;
        Vector2 scale = new (float.Abs (end.X - start.X) + _thickness * 2f, float.Abs (end.Y - start.Y) + _thickness * 2f);

        _mesh.SetUVs ([new Vector3 (position, _depth)]);
        _mesh.SetUV1s ([new Vector4 (_rotation, scale.X, scale.Y, _thickness)]);
        _mesh.SetUV2s ([new Vector4 (start.X - position.X, start.Y - position.Y, end.X - position.X, end.Y - position.Y)]);
        _mesh.SetColors ([_color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (RenderCommand.CreateSingleTexture (Material.SdfLine, _mesh, _depth));
    }
}
