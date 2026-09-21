using System;
using System.Runtime.CompilerServices;

namespace MonoGame.Library.Physics;

public class Collision (PhysicsBody self, PhysicsBody other) : IEquatable<Collision>
{
    public PhysicsBody Self { get; } = self;

    public PhysicsBody Other { get; } = other;

    public Collision Reverse () => new (Other, Self);

    public bool TryGetContact (out Contact contact)
    {
        if (Self.Collider == null || Other.Collider == null)
        {
            contact = default;
            return false;
        }

        return Self.Collider.TryGetContact (Other.Collider, out contact);
    }

    public bool Equals (Collision? other)
    {
        return other != null
            && ((ReferenceEquals (Self, other.Self) && ReferenceEquals (Other, other.Other))
            || (ReferenceEquals (Self, other.Other) && ReferenceEquals (Other, other.Self)));
    }

    public override bool Equals (object? obj)
    {
        return Equals (obj as Collision);
    }

    public override int GetHashCode ()
    {
        return RuntimeHelpers.GetHashCode (Self) ^ RuntimeHelpers.GetHashCode (Other);
    }
}
