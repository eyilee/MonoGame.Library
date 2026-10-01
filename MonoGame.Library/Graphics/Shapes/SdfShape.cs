namespace MonoGame.Library.Graphics.Shapes;

public abstract class SdfShape : Shape
{
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

    protected float _rotation = 0f;

    protected float _thickness = 1f;
}
