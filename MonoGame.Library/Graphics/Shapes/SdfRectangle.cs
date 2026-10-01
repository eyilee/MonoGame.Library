using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics.Shapes;

public class SdfRectangle : SdfShape
{
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

    protected Vector2 _position = Vector2.Zero;

    protected Vector2 _size = Vector2.Zero;

    protected bool _filled = false;

    protected override void PopulateMesh ()
    {
        Vector2 scale = new ((_size.X * 0.5f + _thickness) * 2f, (_size.Y * 0.5f + _thickness) * 2f);

        _mesh.SetUVs ([_position]);
        _mesh.SetUV1s ([new Vector4 (_rotation, scale.X, scale.Y, _thickness)]);
        _mesh.SetUV2s ([_size * 0.5f]);
        _mesh.SetColors ([_color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (new RenderCommand (Filled ? Materials.SdfFilledRectangle : Materials.SdfRectangle, _mesh, _depth));
    }
}
