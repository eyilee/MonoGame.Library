using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class QuadTree (int depth, int capacity, BoundingBox2D bounds, Stack<QuadTree>? pool = null)
{
    public int Depth { get; set; } = depth;

    public int Capacity { get; set; } = capacity;

    public BoundingBox2D Bounds { get; set; } = bounds;

    private readonly List<Collider> _colliders = [];

    private readonly List<QuadTree> _subTrees = [];

    private readonly Stack<QuadTree> _pool = pool ?? new Stack<QuadTree> ();

    public void Reset ()
    {
        _colliders.Clear ();

        foreach (QuadTree quadTree in _subTrees)
        {
            quadTree.Reset ();
            _pool.Push (quadTree);
        }

        _subTrees.Clear ();
    }

    public void Insert (Collider collider) => _colliders.Add (collider);

    public bool TryInsert (Collider collider)
    {
        if (!Bounds.Contains (collider.Bounds))
        {
            return false;
        }

        if (Depth == 0 || _colliders.Count < Capacity)
        {
            _colliders.Add (collider);

            return true;
        }

        if (_subTrees.Count == 0)
        {
            SubDivide ();
        }

        foreach (QuadTree quadTree in _subTrees)
        {
            if (quadTree.TryInsert (collider))
            {
                return true;
            }
        }

        _colliders.Add (collider);

        return true;
    }

    private void SubDivide ()
    {
        Vector2 extents = Bounds.Size / 2f;
        Vector2 leftTop = Bounds.Min;
        Vector2 rightTop = Bounds.Min + new Vector2 (extents.X, 0f);
        Vector2 leftBottom = Bounds.Min + new Vector2 (0f, extents.Y);
        Vector2 rightBottom = Bounds.Min + extents;

        _subTrees.Capacity = 4;
        _subTrees.Add (Create (Depth - 1, Capacity, new BoundingBox2D (leftTop, leftTop + extents)));
        _subTrees.Add (Create (Depth - 1, Capacity, new BoundingBox2D (rightTop, rightTop + extents)));
        _subTrees.Add (Create (Depth - 1, Capacity, new BoundingBox2D (leftBottom, leftBottom + extents)));
        _subTrees.Add (Create (Depth - 1, Capacity, new BoundingBox2D (rightBottom, rightBottom + extents)));
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

    public void GetCollisions (List<Collision> collisions, List<Collider>? ancestorColliders = null)
    {
        if (ancestorColliders != null)
        {
            foreach (Collider collider in _colliders)
            {
                foreach (Collider ancestorCollider in ancestorColliders)
                {
                    if (collider.Bounds.Intersects (ancestorCollider.Bounds))
                    {
                        collisions.Add (new Collision (collider, ancestorCollider));
                    }
                }
            }
        }

        for (int i = 0; i < _colliders.Count; i++)
        {
            Collider colliderA = _colliders[i];

            for (int j = i + 1; j < _colliders.Count; j++)
            {
                Collider colliderB = _colliders[j];

                if (colliderA.Bounds.Intersects (colliderB.Bounds))
                {
                    collisions.Add (new Collision (colliderA, colliderB));
                }
            }
        }

        if (_subTrees.Count == 0)
        {
            return;
        }

        List<Collider> nextAncestorColliderss = [.. ancestorColliders ?? [], .. _colliders];

        foreach (QuadTree quadTree in _subTrees)
        {
            quadTree.GetCollisions (collisions, nextAncestorColliderss);
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
