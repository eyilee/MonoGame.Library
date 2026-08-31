using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

public class Collider
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

    public Vector2 Offset
    {
        get => _offset;
        set
        {
            if (_offset != value)
            {
                _offset = value;
                _dirty = true;
            }
        }
    }

    public Vector2 Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                _dirty = true;
            }
        }
    }

    public BoundingBox2D Bounds
    {
        get
        {
            if (_dirty)
            {
                _bounds = CalculateBounds ();
                _dirty = false;
            }

            return _bounds;
        }
    }

    private Vector2 _position;

    private float _rotation;

    private Vector2 _offset;

    private Vector2 _size;

    private BoundingBox2D _bounds;

    private bool _dirty = true;

    private BoundingBox2D CalculateBounds ()
    {
        Vector2 halfSize = Size / 2f;

        float cos = float.Abs (float.Cos (_rotation));
        float sin = float.Abs (float.Sin (_rotation));

        float halfWidth = cos * halfSize.X + sin * halfSize.Y;
        float halfHeight = sin * halfSize.X + cos * halfSize.Y;

        Vector2 center = _position + _offset;
        Vector2 extents = new (halfWidth, halfHeight);

        return new BoundingBox2D (center - extents, center + extents);
    }
}
