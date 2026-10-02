using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

public struct BoundingBox2D (Vector2 min, Vector2 max)
{
    public Vector2 Min { get; set; } = min;

    public Vector2 Max { get; set; } = max;

    public readonly Vector2 Center => (Min + Max) / 2f;

    public readonly Vector2 Size => Max - Min;

    public readonly bool Contains (Vector2 point) => point.X >= Min.X && point.X <= Max.X && point.Y >= Min.Y && point.Y <= Max.Y;

    public readonly bool Contains (BoundingBox2D other) => Contains (other.Min) && Contains (other.Max);

    public readonly bool Intersects (BoundingBox2D other) => !(other.Min.X > Max.X || other.Max.X < Min.X || other.Min.Y > Max.Y || other.Max.Y < Min.Y);

    public readonly BoundingBox2D Encapsulate (BoundingBox2D other) => new (Vector2.Min (Min, other.Min), Vector2.Max (Max, other.Max));

    public readonly BoundingBox2D Expand (BoundingBox2D other) => new (Min + other.Size / 2f, Max + other.Size / 2f);
}
