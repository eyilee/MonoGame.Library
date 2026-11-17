using System;
using System.Runtime.CompilerServices;

namespace Eycraft.Framework.Physics;

public readonly struct ColliderPair (Collider self, Collider other) : IEquatable<ColliderPair>
{
    public Collider Self { get; } = self;

    public Collider Other { get; } = other;

    public void Deconstruct (out Collider self, out Collider other)
    {
        self = Self;
        other = Other;
    }

    public static bool operator == (ColliderPair lhs, ColliderPair rhs) => lhs.Equals (rhs);

    public static bool operator != (ColliderPair lhs, ColliderPair rhs) => !lhs.Equals (rhs);

    public bool Equals (ColliderPair other)
    {
        return ReferenceEquals (Self, other.Self) && ReferenceEquals (Other, other.Other)
            || ReferenceEquals (Self, other.Other) && ReferenceEquals (Other, other.Self);
    }

    public override bool Equals (object? obj)
    {
        return obj is ColliderPair other && Equals (other);
    }

    public override readonly int GetHashCode ()
    {
        return RuntimeHelpers.GetHashCode (Self) ^ RuntimeHelpers.GetHashCode (Other);
    }
}
