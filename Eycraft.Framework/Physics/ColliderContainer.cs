using System;
using System.Collections.Generic;

namespace Eycraft.Framework.Physics;

internal sealed class ColliderContainer (PhysicsBody owner)
{
    public IReadOnlySet<Collider> Colliders => _colliders;

    private readonly PhysicsBody _physicsBody = owner;

    private readonly HashSet<Collider> _colliders = [];

    public T AddCollider<T> () where T : Collider
    {
        Type type = typeof (T);

        if (Activator.CreateInstance (type, _physicsBody) is not Collider collider)
        {
            throw new InvalidOperationException ($"Type {type.Name} is not a valid Collider.");
        }

        _colliders.Add (collider);

        return (T)collider;
    }

    public void RemoveCollider (Collider collider) => _colliders.Remove (collider);

    public bool Contains (Collider collider) => _colliders.Contains (collider);
}
