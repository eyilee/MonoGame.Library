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

    public float Left
    {
        get { return _left; }
        set
        {
            if (_left != value)
            {
                _left = value;
                _dirty = true;
            }
        }
    }

    public float Top
    {
        get { return _top; }
        set
        {
            if (_top != value)
            {
                _top = value;
                _dirty = true;
            }
        }
    }

    public float Right
    {
        get { return _right; }
        set
        {
            if (_right != value)
            {
                _right = value;
                _dirty = true;
            }
        }
    }

    public float Bottom
    {
        get { return _bottom; }
        set
        {
            if (_bottom != value)
            {
                _bottom = value;
                _dirty = true;
            }
        }
    }

    protected Vector2 _focus = Vector2.Zero;

    protected Vector2 _vertex = Vector2.Zero;

    protected float _left = 0f;

    protected float _top = 0f;

    protected float _right = 0f;

    protected float _bottom = 0f;

    protected override void PopulateMesh ()
    {
        //Vector2 upDirection = _focus - _vertex;
        //upDirection.Rotate (_rotation);
        //upDirection.Normalize ();

        //Vector2 leftDirection = -upDirection.Perpendicular ();
        //Vector2 leftTop = _vertex + _left * leftDirection + _top * upDirection;
        //Vector2 rightBottom = _vertex + _right * -leftDirection + _bottom * -upDirection;
        //Vector2 position = (leftTop + rightBottom) * 0.5f;
        //Vector2 scale = new (_left + _right, _top + _bottom);
        //Vector2 offset = position - _vertex;

        Vector2 leftTop = new (_vertex.X - _left, _vertex.Y - _top);
        Vector2 rightBottom = new (_vertex.X + _right, _vertex.Y + _bottom);
        Vector2 position = (leftTop + rightBottom) * 0.5f;
        Vector2 scale = new (_left + _right, _top + _bottom);
        Vector2 offset = position - _vertex;

        float angle = float.Atan2 (_focus.Y - _vertex.Y, _focus.X - _vertex.X) - (float.Pi / 2f);

        _mesh.SetUVs ([position]);
        _mesh.SetUV1s ([new Vector4 (_rotation, scale.X, scale.Y, _thickness)]);
        _mesh.SetUV2s ([new Vector4 (1f / (4f * Vector2.Distance (_focus, _vertex)), offset.X, offset.Y, -angle)]);
        _mesh.SetColors ([_color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (new RenderCommand (render.SdfParabolaMaterial, _mesh, _depth));
    }
}
