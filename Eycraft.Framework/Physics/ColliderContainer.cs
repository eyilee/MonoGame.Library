using System;
using System.Collections.Generic;

namespace Eycraft.Framework.Physics;

internal sealed class ColliderContainer (PhysicsBody owner)
{
    public IReadOnlyList<Collider> Colliders => _colliders;

    private readonly PhysicsBody _physicsBody = owner;

    private readonly List<Collider> _colliders = [];

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

    public void RemoveCollider<T> () where T : Collider
    {
        int index = _colliders.FindIndex (x => x is T);

        if (index == -1)
        {
            return;
        }

        _colliders.RemoveAt (index);
    }

    public void RemoveColliders<T> () where T : Collider
    {
        _colliders.RemoveAll (x => x is T);
    }

    public void RemoveCollider (Collider collider)
    {
        _colliders.Remove (collider);
    }
}
