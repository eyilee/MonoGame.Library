using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics;

public class SdfLine : SdfShape
{
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

    protected Vector2 _start = Vector2.Zero;

    protected Vector2 _end = Vector2.Zero;

    protected override void PopulateMesh ()
    {
        _position = (_start + _end) * 0.5f;
        _scale = new (float.Abs (_end.X - _start.X) + _thickness * 2f, float.Abs (_end.Y - _start.Y) + _thickness * 2f);

        _mesh.SetUVs ([_position]);
        _mesh.SetUV1s ([new Vector4 (_rotation, _scale.X, _scale.Y, _thickness)]);
        _mesh.SetUV2s ([new Vector4 (_start.X - _position.X, _start.Y - _position.Y, _end.X - _position.X, _end.Y - _position.Y)]);
        _mesh.SetColors ([_color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (new RenderCommand (Materials.SdfLine, _mesh, _depth));
    }
}
