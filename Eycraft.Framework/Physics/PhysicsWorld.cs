using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Eycraft.Framework.Physics;

public class PhysicsWorld
{
    private readonly HashSet<PhysicsBody> _bodies = [];

    private readonly HashSet<ColliderPair> _potentialCollisions = [];

    private Dictionary<ColliderPair, Collision> _previousCollisions = [];

    private Dictionary<ColliderPair, Collision> _currentCollisions = [];

    private readonly QuadTree _quadTree = new (4, 4, new BoundingBox2D (new Vector2 (-1000f, -1000f), new Vector2 (1000f, 1000f)));

    private PhysicsResolver Resolver { get; } = new ();

    public void Add (PhysicsBody body) => _bodies.Add (body);

    public void Remove (PhysicsBody body) => _bodies.Remove (body);

    public void Update (GameTime gameTime)
    {
        _quadTree.Reset ();

        foreach (PhysicsBody body in _bodies)
        {
            foreach (Collider collider in body.Colliders)
            {
                if (!_quadTree.TryInsert (collider))
                {
                    _quadTree.Insert (collider);
                }
            }
        }

        _potentialCollisions.Clear ();
        _quadTree.GetPotentialCollisions (_potentialCollisions);

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

        Resolver.Resolve (_currentCollisions);

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
}
