using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

public class Collider (PhysicsEntity entity)
{
    public PhysicsEntity Entity { get; } = entity;

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

    private Vector2 _size;

    private BoundingBox2D _bounds;

    private bool _dirty = true;

    public void SetDirty () => _dirty = true;

    private BoundingBox2D CalculateBounds ()
    {
        Vector2 halfSize = Size / 2f;

        float cos = float.Abs (float.Cos (Entity.Rotation));
        float sin = float.Abs (float.Sin (Entity.Rotation));

        float halfWidth = cos * halfSize.X + sin * halfSize.Y;
        float halfHeight = sin * halfSize.X + cos * halfSize.Y;

        return new BoundingBox2D (Entity.Position - new Vector2 (halfWidth, halfHeight), Entity.Position + new Vector2 (halfWidth, halfHeight));
    }
}
