using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics.Shapes;

public abstract class SdfShape : Shape
{
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

    protected Vector2 _scale = Vector2.Zero;

    protected bool _filled = false;
}
