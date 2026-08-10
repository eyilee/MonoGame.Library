using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics;

public abstract class SdfShape
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

    public Vector2 Scale
    {
        get => _scale;
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
        get => _thickness;
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
        get => _color;
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
        get => _depth;
        set
        {
            if (_depth != value)
            {
                _depth = value;
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

    protected readonly Mesh _mesh = new ();

    protected Vector2 _position = Vector2.Zero;

    protected Vector2 _scale = Vector2.Zero;

    protected float _rotation = 0f;

    protected float _thickness = 1f;

    protected Color _color = Color.White;

    protected float _depth = 0f;

    protected bool _dirty = true;

    protected bool _filled = false;

    protected abstract void PopulateMesh ();

    public abstract void Draw (RenderManager render);
}
