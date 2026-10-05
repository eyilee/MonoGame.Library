using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class PhysicsWorld
{
    private readonly HashSet<PhysicsBody> _bodies = [];

    private HashSet<Collision> _previousCollisions = [];

    private HashSet<Collision> _collisions = [];

    private readonly QuadTree _quadTree = new (4, 4, new BoundingBox2D (new Vector2 (-1000f, -1000f), new Vector2 (1000f, 1000f)));

    public PhysicsResolver Resolver { get; } = new ();

    public void Add (PhysicsBody body) => _bodies.Add (body);

    public void Remove (PhysicsBody body) => _bodies.Remove (body);

    public void Update (float deltaTime)
    {
        _quadTree.Reset ();

        foreach (PhysicsBody body in _bodies)
        {
            if (!_quadTree.TryInsert (body))
            {
                _quadTree.Insert (body);
            }
        }

        _collisions.Clear ();
        _quadTree.GetCollisions (_collisions);

        _collisions.RemoveWhere (x => !x.Intersects);

        Resolver.Resolve (_collisions);

        foreach (Collision collision in _collisions)
        {
            if (_previousCollisions.Contains (collision))
            {
                collision.Self.Owner.OnCollisionStay (collision);
                collision.Other.Owner.OnCollisionStay (collision.Reverse ());
            }
            else
            {
                collision.Self.Owner.OnCollisionEnter (collision);
                collision.Other.Owner.OnCollisionEnter (collision.Reverse ());
            }
        }

        foreach (Collision collision in _previousCollisions)
        {
            if (!_collisions.Contains (collision))
            {
                collision.Self.Owner.OnCollisionExit (collision);
                collision.Other.Owner.OnCollisionExit (collision.Reverse ());
            }
        }

        (_collisions, _previousCollisions) = (_previousCollisions, _collisions);
    }
}
