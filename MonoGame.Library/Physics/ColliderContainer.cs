using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public sealed class ColliderContainer (PhysicsBody owner)
{
    public IReadOnlyList<Collider> Colliders => _colliders;

    private readonly PhysicsBody _physicsBody = owner;

    private readonly List<Collider> _colliders = [];

    public T AddCollider<T> () where T : Collider, new()
    {
        T collider = new ()
        {
            PhysicsBody = _physicsBody
        };

        _colliders.Add (collider);

        return collider;
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
