using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class PhysicsWorld
{
    private readonly HashSet<PhysicsBody> _bodies = [];

    private readonly HashSet<ColliderCollision> _colliderCollisions = [];

    private HashSet<PhysicsBodyCollision> _previousBodyCollisions = [];

    private HashSet<PhysicsBodyCollision> _currentBodyCollisions = [];

    private readonly QuadTree _quadTree = new (4, 4, new BoundingBox2D (new Vector2 (-1000f, -1000f), new Vector2 (1000f, 1000f)));

    public PhysicsResolver Resolver { get; } = new ();

    public void Add (PhysicsBody body) => _bodies.Add (body);

    public void Remove (PhysicsBody body) => _bodies.Remove (body);

    public void Update (float deltaTime)
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

        _colliderCollisions.Clear ();
        _quadTree.GetCollisions (_colliderCollisions);
        _colliderCollisions.RemoveWhere (x => !x.Intersects);

        Resolver.Resolve (_colliderCollisions);

        _currentBodyCollisions.Clear ();

        foreach (ColliderCollision collision in _colliderCollisions)
        {
            _currentBodyCollisions.Add (new PhysicsBodyCollision (collision.Self.PhysicsBody, collision.Other.PhysicsBody));
        }

        foreach (PhysicsBodyCollision collision in _currentBodyCollisions)
        {
            if (_previousBodyCollisions.Contains (collision))
            {
                collision.Self.InvokeCollisionStay (collision);
                collision.Other.InvokeCollisionStay (collision.Reverse);
            }
            else
            {
                collision.Self.InvokeCollisionEnter (collision);
                collision.Other.InvokeCollisionEnter (collision.Reverse);
            }
        }

        foreach (PhysicsBodyCollision collision in _previousBodyCollisions)
        {
            if (!_currentBodyCollisions.Contains (collision))
            {
                collision.Self.InvokeCollisionExit (collision);
                collision.Other.InvokeCollisionExit (collision.Reverse);
            }
        }

        (_currentBodyCollisions, _previousBodyCollisions) = (_previousBodyCollisions, _currentBodyCollisions);
    }
}
