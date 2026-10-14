using System;
using System.Runtime.CompilerServices;

namespace MonoGame.Library.Physics;

public class ColliderCollision (Collider self, Collider other) : IEquatable<ColliderCollision>
{
    public Collider Self { get; } = self;

    public Collider Other { get; } = other;

    public ColliderCollision Reverse
    {
        get => _reverse ??= new (Other, Self);
        set => _reverse = value;
    }

    private ColliderCollision _reverse = null!;

    public bool Intersects => Self.Intersects (Other);

    public bool TryGetContacts (out Contact selfContact, out Contact otherContact) => Self.TryGetContacts (Other, out selfContact, out otherContact);

    public bool Equals (ColliderCollision? other)
    {
        return other != null
            && ((ReferenceEquals (Self, other.Self) && ReferenceEquals (Other, other.Other))
            || (ReferenceEquals (Self, other.Other) && ReferenceEquals (Other, other.Self)));
    }

    public override bool Equals (object? obj)
    {
        return Equals (obj as ColliderCollision);
    }

    public override int GetHashCode ()
    {
        return RuntimeHelpers.GetHashCode (Self) ^ RuntimeHelpers.GetHashCode (Other);
    }
}
