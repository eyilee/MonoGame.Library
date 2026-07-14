using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics;

public abstract class SdfShape
{
    public Vector2 Position
    {
        get { return _position; }
        set
        {
            if (_position != value)
            {
                _position = value;
                _dirty = true;
            }
        }
    }

    public float Rotation
    {
        get { return _rotation; }
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                _dirty = true;
            }
        }
    }

    public Vector2 Scale
    {
        get { return _scale; }
        set
        {
            if (_scale != value)
            {
                _scale = value;
                _dirty = true;
            }
        }
    }

    public float Thickness
    {
        get { return _thickness; }
        set
        {
            if (_thickness != value)
            {
                _thickness = value;
                _dirty = true;
            }
        }
    }

    public Color Color
    {
        get { return _color; }
        set
        {
            if (_color != value)
            {
                _color = value;
                _dirty = true;
            }
        }
    }

    public float Depth
    {
        get { return _depth; }
        set
        {
            if (_depth != value)
            {
                _depth = value;
                _dirty = true;
            }
        }
    }

    protected readonly Mesh _mesh = new ();

    protected Vector2 _position = Vector2.Zero;

    protected Vector2 _scale = Vector2.Zero;

    protected float _rotation = 0f;

    protected float _thickness = 1f;

    protected Color _color = Color.White;

    protected float _depth = 0f;

    protected bool _dirty;

    protected abstract void PopulateMesh ();

    public abstract void Draw (RenderManager render);
}
