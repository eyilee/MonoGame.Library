using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

public struct Circle (Vector2 point, float radius) : ISupportable
{
    public Vector2 Point = point;

    public float Radius = radius;

    public readonly Vector2 Center => Point;

    public readonly Vector2 Support (Vector2 direction)
    {
        return Point + Vector2.Normalize (direction) * Radius;
    }
}
