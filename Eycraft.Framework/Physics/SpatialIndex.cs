using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Eycraft.Framework.Physics;

internal class SpatialNode
{
    public int Depth { get; set; }

    public int Capacity { get; set; }

    public BoundingBox2D Bounds { get; set; }

    public List<Collider> Colliders { get; } = [];

    public SpatialNode? Parent { get; set; }

    public SpatialNode? Next { get; set; }

    public SpatialNode? Child { get; set; }
}

internal class SpatialIndex
{
    private readonly SpatialNode _root = new ();

    private readonly Stack<SpatialNode> _pool = new ();

    public SpatialIndex (int maxDepth, int nodeCapacity, BoundingBox2D bounds)
    {
        _root.Depth = maxDepth;
        _root.Capacity = nodeCapacity;
        _root.Bounds = bounds;
    }

    public void Clear ()
    {
        SpatialNode? current = _root;

        while (current != null)
        {
            if (current.Child != null)
            {
                current = current.Child;
                continue;
            }

            SpatialNode? parent = current.Parent;
            SpatialNode? next = current.Next;

            current.Colliders.Clear ();
            current.Parent = null;
            current.Next = null;
            current.Child = null;

            if (current != _root)
            {
                _pool.Push (current);
            }

            if (next != null)
            {
                current = next;
            }
            else if (parent != null)
            {
                current = parent;
                current.Child = null;
            }
            else
            {
                break;
            }
        }
    }

    public void InsertAtRoot (Collider collider) => _root.Colliders.Add (collider);

    public bool TryInsert (Collider collider)
    {
        SpatialNode? current = _root;
        SpatialNode? target = null;

        while (current != null)
        {
            if (!current.Bounds.Contains (collider.Bounds))
            {
                current = current.Next;
                continue;
            }

            target = current;

            if (current.Depth == 0 || current.Colliders.Count < current.Capacity)
            {
                break;
            }

            if (current.Child == null)
            {
                Vector2 extents = current.Bounds.Size / 2f;
                Vector2 leftTop = current.Bounds.Min;
                Vector2 rightTop = current.Bounds.Min + new Vector2 (extents.X, 0f);
                Vector2 leftBottom = current.Bounds.Min + new Vector2 (0f, extents.Y);
                Vector2 rightBottom = current.Bounds.Min + extents;

                SpatialNode child = Create (current.Depth - 1, current.Capacity, new BoundingBox2D (leftTop, leftTop + extents));
                current.Child = child;
                current.Child.Parent = current;

                child.Next = (Create (current.Depth - 1, current.Capacity, new BoundingBox2D (rightTop, rightTop + extents)));
                child = child.Next;
                child.Parent = current;

                child.Next = (Create (current.Depth - 1, current.Capacity, new BoundingBox2D (leftBottom, leftBottom + extents)));
                child = child.Next;
                child.Parent = current;

                child.Next = (Create (current.Depth - 1, current.Capacity, new BoundingBox2D (rightBottom, rightBottom + extents)));
                child = child.Next;
                child.Parent = current;
            }

            current = current.Child;
        }

        if (target != null)
        {
            target.Colliders.Add (collider);

            return true;
        }

        return false;
    }

    private SpatialNode Create (int depth, int capacity, BoundingBox2D bounds)
    {
        if (!_pool.TryPop (out SpatialNode? node))
        {
            node = new SpatialNode ();
        }

        node.Depth = depth;
        node.Capacity = capacity;
        node.Bounds = bounds;
        node.Parent = null;
        node.Next = null;
        node.Child = null;

        return node;
    }

    public void GetPotentialCollisions (HashSet<ColliderPair> potentialCollisions)
    {
        SpatialNode? current = _root;

        while (current != null)
        {
            foreach (Collider collider in current.Colliders)
            {
                SpatialNode? ancestor = current.Parent;

                while (ancestor != null)
                {
                    foreach (Collider ancestorCollider in ancestor.Colliders)
                    {
                        if (collider.PhysicsBody == ancestorCollider.PhysicsBody)
                        {
                            continue;
                        }

                        if (collider.Bounds.Intersects (ancestorCollider.Bounds))
                        {
                            potentialCollisions.Add (new (collider, ancestorCollider));
                        }
                    }

                    ancestor = ancestor.Parent;
                }
            }

            for (int i = 0; i < current.Colliders.Count; i++)
            {
                Collider collider1 = current.Colliders[i];

                for (int j = i + 1; j < current.Colliders.Count; j++)
                {
                    Collider collider2 = current.Colliders[j];

                    if (collider1.PhysicsBody == collider2.PhysicsBody)
                    {
                        continue;
                    }

                    if (collider1.Bounds.Intersects (collider2.Bounds))
                    {
                        potentialCollisions.Add (new (collider1, collider2));
                    }
                }
            }

            if (current.Child != null)
            {
                current = current.Child;
                continue;
            }

            while (current != null)
            {
                if (current.Next != null)
                {
                    current = current.Next;
                    break;
                }

                current = current.Parent;
            }
        }
    }
}
