using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class PhysicsWorld
{
    private readonly List<PhysicsEntity> _entities = [];

    private readonly List<Collision> _collisions = [];

    private readonly QuadTree _quadTree = new (4, 4, new BoundingBox2D (new Vector2 (-1000f, -1000f), new Vector2 (1000f, 1000f)));

    public void Add (PhysicsEntity entity) => _entities.Add (entity);

    public void Remove (PhysicsEntity entity) => _entities.Remove (entity);

    public void Update (float deltaTime)
    {
        _quadTree.Reset ();

        foreach (PhysicsEntity entity in _entities)
        {
            if (!_quadTree.TryInsert (entity.Collider))
            {
                _quadTree.Insert (entity.Collider);
            }
        }

        _quadTree.GetCollisions (_collisions);
    }
}
