using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics;

public class SdfParabola : SdfShape
{
    public Vector2 Focus
    {
        get { return _focus; }
        set
        {
            if (_focus != value)
            {
                _focus = value;
                _dirty = true;
            }
        }
    }

    public Vector2 Vertex
    {
        get { return _vertex; }
        set
        {
            if (_vertex != value)
            {
                _vertex = value;
                _dirty = true;
            }
        }
    }

    protected Vector2 _focus = Vector2.Zero;

    protected Vector2 _vertex = Vector2.Zero;

    protected override void PopulateMesh ()
    {
        float angle = float.Atan2 (_focus.Y - _vertex.Y, _focus.X - _vertex.X) - (float.Pi / 2f);
        Vector2 offset = _vertex - _position;

        _mesh.SetUVs ([_position]);
        _mesh.SetUV1s ([new Vector4 (_rotation, _scale.X, _scale.Y, _thickness)]);
        _mesh.SetUV2s ([new Vector4 (-angle, offset.X, offset.Y, 1f / (4f * Vector2.Distance (_focus, _vertex)))]);
        _mesh.SetColors ([_color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (new RenderCommand (Materials.SdfParabola, _mesh, _depth));
    }
}
