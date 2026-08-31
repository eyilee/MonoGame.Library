using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class QuadTree (int depth, int capacity, BoundingBox2D bounds, Stack<QuadTree>? pool = null)
{
    public int Depth { get; set; } = depth;

    public int Capacity { get; set; } = capacity;

    public BoundingBox2D Bounds { get; set; } = bounds;

    private readonly List<PhysicsBody> _bodies = [];

    private readonly List<QuadTree> _nodes = [];

    private readonly Stack<QuadTree> _pool = pool ?? new Stack<QuadTree> ();

    public void Reset ()
    {
        _bodies.Clear ();

        foreach (QuadTree quadTree in _nodes)
        {
            quadTree.Reset ();
            _pool.Push (quadTree);
        }

        _nodes.Clear ();
    }

    public void Insert (PhysicsBody body) => _bodies.Add (body);

    public bool TryInsert (PhysicsBody body)
    {
        if (!Bounds.Contains (body.Collider.Bounds))
        {
            return false;
        }

        if (Depth == 0 || _bodies.Count < Capacity)
        {
            _bodies.Add (body);

            return true;
        }

        if (_nodes.Count == 0)
        {
            SubDivide ();
        }

        foreach (QuadTree quadTree in _nodes)
        {
            if (quadTree.TryInsert (body))
            {
                return true;
            }
        }

        _bodies.Add (body);

        return true;
    }

    private void SubDivide ()
    {
        Vector2 extents = Bounds.Size / 2f;
        Vector2 leftTop = Bounds.Min;
        Vector2 rightTop = Bounds.Min + new Vector2 (extents.X, 0f);
        Vector2 leftBottom = Bounds.Min + new Vector2 (0f, extents.Y);
        Vector2 rightBottom = Bounds.Min + extents;

        _nodes.Capacity = 4;
        _nodes.Add (Create (Depth - 1, Capacity, new BoundingBox2D (leftTop, leftTop + extents)));
        _nodes.Add (Create (Depth - 1, Capacity, new BoundingBox2D (rightTop, rightTop + extents)));
        _nodes.Add (Create (Depth - 1, Capacity, new BoundingBox2D (leftBottom, leftBottom + extents)));
        _nodes.Add (Create (Depth - 1, Capacity, new BoundingBox2D (rightBottom, rightBottom + extents)));
    }

    private QuadTree Create (int depth, int capacity, BoundingBox2D bounds)
    {
        if (_pool.TryPop (out QuadTree? quadTree))
        {
            quadTree.Depth = depth;
            quadTree.Capacity = capacity;
            quadTree.Bounds = bounds;

            return quadTree;
        }

        return new QuadTree (depth, capacity, bounds, _pool);
    }

    public void GetCollisions (HashSet<Collision> collisions, List<PhysicsBody>? ancestorBodies = null)
    {
        if (ancestorBodies != null)
        {
            foreach (PhysicsBody body in _bodies)
            {
                foreach (PhysicsBody ancestorBody in ancestorBodies)
                {
                    if (body.Collider.Bounds.Intersects (ancestorBody.Collider.Bounds))
                    {
                        collisions.Add (new Collision (body, ancestorBody));
                    }
                }
            }
        }

        for (int i = 0; i < _bodies.Count; i++)
        {
            PhysicsBody bodyA = _bodies[i];

            for (int j = i + 1; j < _bodies.Count; j++)
            {
                PhysicsBody bodyB = _bodies[j];

                if (bodyA.Collider.Bounds.Intersects (bodyB.Collider.Bounds))
                {
                    collisions.Add (new Collision (bodyA, bodyB));
                }
            }
        }

        if (_nodes.Count == 0)
        {
            return;
        }

        List<PhysicsBody> nextAncestorBodies = [.. ancestorBodies ?? [], .. _bodies];

        foreach (QuadTree quadTree in _nodes)
        {
            quadTree.GetCollisions (collisions, nextAncestorBodies);
        }
    }

    //public void Query (NgCollider2D target, List<NgCollider2D> colliders)
    //{
    //    if (target.BoundingBox.Intersects (m_BoundingBox))
    //    {
    //        foreach (NgCollider2D collider in m_Colliders)
    //        {
    //            if ((target.LayerMask & collider.LayerMask) == 0)
    //            {
    //                continue;
    //            }

    //            if (!target.Equals (collider) && target.BoundingBox.Intersects (collider.BoundingBox))
    //            {
    //                colliders.Add (collider);
    //            }
    //        }

    //        foreach (QuadTree quadTree in m_QuadTrees)
    //        {
    //            quadTree.Query (target, colliders);
    //        }
    //    }
    //}
}
