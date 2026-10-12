using System;
using System.Runtime.CompilerServices;

namespace MonoGame.Library.Physics;

public class PhysicsBodyCollision (PhysicsBody self, PhysicsBody other) : IEquatable<PhysicsBodyCollision>
{
    public PhysicsBody Self { get; } = self;

    public PhysicsBody Other { get; } = other;

    public PhysicsBodyCollision Reverse
    {
        get => _reverse ??= new (Other, Self);
        set => _reverse = value;
    }

    private PhysicsBodyCollision _reverse = null!;

    public bool Equals (PhysicsBodyCollision? other)
    {
        return other != null
            && ((ReferenceEquals (Self, other.Self) && ReferenceEquals (Other, other.Other))
            || (ReferenceEquals (Self, other.Other) && ReferenceEquals (Other, other.Self)));
    }

    public override bool Equals (object? obj)
    {
        return Equals (obj as PhysicsBodyCollision);
    }

    public override int GetHashCode ()
    {
        return RuntimeHelpers.GetHashCode (Self) ^ RuntimeHelpers.GetHashCode (Other);
    }
}
