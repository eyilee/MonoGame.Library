using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Eycraft.Framework.Physics;

public class PhysicsWorld
{
    private readonly HashSet<PhysicsBody> _staticBodies = [];

    private readonly HashSet<PhysicsBody> _dynamicBodies = [];

    private readonly HashSet<PhysicsBody> _dirtyBodies = [];

    private readonly HashSet<Collider> _dirtyColliders = [];

    private readonly HashSet<ColliderPair> _potentialCollisions = [];

    private Dictionary<ColliderPair, Collision> _currentCollisions = [];

    private Dictionary<ColliderPair, Collision> _previousCollisions = [];

    private readonly SpatialIndex _staticIndex = new (4, 4, new BoundingBox2D (new Vector2 (-1000f, -1000f), new Vector2 (1000f, 1000f)), trackColliderNodes: true);

    private readonly SpatialIndex _dynamicIndex = new (4, 4, new BoundingBox2D (new Vector2 (-1000f, -1000f), new Vector2 (1000f, 1000f)));

    private readonly PhysicsResolver _resolver = new ();

    internal void Add (PhysicsBody body)
    {
        if (body.IsDynamic || body.IsKinematic)
        {
            _dynamicBodies.Add (body);
        }
        else if (body.IsStatic)
        {
            _staticBodies.Add (body);
            _staticIndex.Remove (body.Colliders);
            _staticIndex.Insert (body.Colliders);
        }
    }

    internal void Remove (PhysicsBody body)
    {
        _dynamicBodies.Remove (body);

        _staticBodies.Remove (body);
        _staticIndex.Remove (body.Colliders);

        _dirtyBodies.Remove (body);

        foreach (Collider collider in body.Colliders)
        {
            _dirtyColliders.Remove (collider);
        }
    }

    internal void SetDirty (PhysicsBody body) => _dirtyBodies.Add (body);

    internal void SetDirty (Collider collider) => _dirtyColliders.Add (collider);

    public void Update (GameTime gameTime)
    {
        UpdateDirtyBodies ();
        UpdateDirtyColliders ();
        UpdateDynamicIndex ();

        _potentialCollisions.Clear ();
        _dynamicIndex.GetPotentialCollisions (_staticIndex, _potentialCollisions);
        _dynamicIndex.GetPotentialCollisions (_potentialCollisions);

        _currentCollisions.Clear ();

        foreach (ColliderPair pairKey in _potentialCollisions)
        {
            (Collider self, Collider other) = pairKey;

            if (self.TryGetContacts (other, out Contact selfContact, out Contact otherContact))
            {
                if (!_currentCollisions.ContainsKey (pairKey))
                {
                    _currentCollisions.Add (pairKey, new Collision (self, other, selfContact, otherContact));
                }
            }
        }

        _resolver.Resolve (_currentCollisions);

        foreach ((ColliderPair pairKey, Collision collision) in _currentCollisions)
        {
            if (_previousCollisions.ContainsKey (pairKey))
            {
                collision.Self.PhysicsBody.InvokeCollisionStay (collision);
                collision.Other.PhysicsBody.InvokeCollisionStay (new Collision (collision.Other, collision.Self, collision.OtherContact, collision.SelfContact));
            }
            else
            {
                collision.Self.PhysicsBody.InvokeCollisionEnter (collision);
                collision.Other.PhysicsBody.InvokeCollisionEnter (new Collision (collision.Other, collision.Self, collision.OtherContact, collision.SelfContact));
            }
        }

        foreach ((ColliderPair pairKey, Collision collision) in _previousCollisions)
        {
            if (!_currentCollisions.ContainsKey (pairKey))
            {
                collision.Self.PhysicsBody.InvokeCollisionExit (collision);
                collision.Other.PhysicsBody.InvokeCollisionExit (new Collision (collision.Other, collision.Self, collision.OtherContact, collision.SelfContact));
            }
        }

        (_currentCollisions, _previousCollisions) = (_previousCollisions, _currentCollisions);
    }

    private void UpdateDirtyBodies ()
    {
        foreach (PhysicsBody body in _dirtyBodies)
        {
            if (body.IsDynamic || body.IsKinematic)
            {
                if (_staticBodies.Remove (body))
                {
                    _dynamicBodies.Add (body);
                }

                foreach (Collider collider in body.Colliders)
                {
                    _dirtyColliders.Remove (collider);
                    _staticIndex.Remove (collider);
                }
            }
            else if (body.IsStatic)
            {
                if (_dynamicBodies.Remove (body))
                {
                    _staticBodies.Add (body);
                }

                foreach (Collider collider in body.Colliders)
                {
                    _dirtyColliders.Remove (collider);
                    _staticIndex.Remove (collider);
                    _staticIndex.Insert (collider);
                }
            }
        }

        _dirtyBodies.Clear ();
    }

    private void UpdateDirtyColliders ()
    {
        foreach (Collider collider in _dirtyColliders)
        {
            PhysicsBody body = collider.PhysicsBody;

            if (body.IsStatic)
            {
                _staticIndex.Remove (collider);

                if (body.Colliders.Contains (collider))
                {
                    _staticIndex.Insert (collider);
                }
            }
        }

        _dirtyColliders.Clear ();
    }

    private void UpdateDynamicIndex ()
    {
        _dynamicIndex.Clear ();

        foreach (PhysicsBody body in _dynamicBodies)
        {
            foreach (Collider collider in body.Colliders)
            {
                _dynamicIndex.Insert (collider);
            }
        }
    }
}
