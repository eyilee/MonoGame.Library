using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics;

public class SdfRectangle : SdfShape
{
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

    protected Vector2 _size = Vector2.Zero;

    protected override void PopulateMesh ()
    {
        _scale = new Vector2 ((_size.X * 0.5f + _thickness) * 2f, (_size.Y * 0.5f + _thickness) * 2f);

        _mesh.SetUVs ([_position]);
        _mesh.SetUV1s ([new Vector4 (_rotation, _scale.X, _scale.Y, _thickness)]);
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
