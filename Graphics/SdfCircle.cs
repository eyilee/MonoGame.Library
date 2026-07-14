using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics;

public class SdfCircle : SdfShape
{
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

    protected float _radius = 0f;

    protected override void PopulateMesh ()
    {
        _scale = new Vector2 ((_radius + _thickness) * 2f, (_radius + _thickness) * 2f);

        _mesh.SetUVs ([_position]);
        _mesh.SetUV1s ([new Vector4 (_rotation, _scale.X, _scale.Y, _thickness)]);
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

        render.Enqueue (new RenderCommand (render.SdfCircleMaterial, _mesh, _depth));
    }
}
