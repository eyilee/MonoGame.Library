using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics.Shapes;

public class SdfCircle : SdfShape
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

    protected float _radius = 0f;

    protected bool _filled = false;

    protected override void PopulateMesh ()
    {
        Vector2 scale = new ((_radius + _thickness) * 2f, (_radius + _thickness) * 2f);

        _mesh.SetUVs ([_position]);
        _mesh.SetUV1s ([new Vector4 (_rotation, scale.X, scale.Y, _thickness)]);
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

        render.Enqueue (new RenderCommand (Filled ? Materials.SdfFilledCircle : Materials.SdfCircle, _mesh, _depth));
    }
}
