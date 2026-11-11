using System;

namespace Eycraft.Framework;

public struct Direction (int value) : IEquatable<Direction>
{
    public const int None = 0;

    public const int Up = 1;

    public const int Right = 2;

    public const int Down = 3;

    public const int Left = 4;

    public int Value { get; set; } = value;

    public static Direction FromRotation (float rotation)
    {
        float sin = float.Sin (rotation);
        float cos = float.Cos (rotation);

        if (float.Abs (sin) > float.Abs (cos))
        {
            return sin > 0f ? new Direction (Down) : new Direction (Up);
        }
        else
        {
            return cos > 0f ? new Direction (Right) : new Direction (Left);
        }
    }

    public static bool operator == (Direction lhs, Direction rhs) => lhs.Equals (rhs);

    public static bool operator != (Direction lhs, Direction rhs) => !lhs.Equals (rhs);

    public readonly bool Equals (Direction other) => Value == other.Value;

    public override readonly bool Equals (object? obj) => obj is Direction other && Equals (other);

    public override readonly int GetHashCode () => Value.GetHashCode ();
}
