using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

public struct Circle (Vector2 point, float radius) : ISupportable
{
    public Vector2 Point { get; set; } = point;

    public float Radius { get; set; } = radius;

    public readonly Vector2 Position => Point;

    public readonly Vector2 Support (Vector2 direction)
    {
        return Point + Vector2.Normalize (direction) * Radius;
    }
}
