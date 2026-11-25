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

    private readonly Dictionary<Collider, SpatialNode> _colliderNodes = [];

    private readonly bool _trackColliderNodes;

    public SpatialIndex (int maxDepth, int nodeCapacity, BoundingBox2D bounds, bool trackColliderNodes = false)
    {
        _root.Depth = maxDepth;
        _root.Capacity = nodeCapacity;
        _root.Bounds = bounds;
        _trackColliderNodes = trackColliderNodes;
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

        if (_trackColliderNodes)
        {
            _colliderNodes.Clear ();
        }
    }

    public void Insert (Collider collider)
    {
        SpatialNode? current = _root;
        SpatialNode? target = _root;

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

        target.Colliders.Add (collider);

        if (_trackColliderNodes)
        {
            _colliderNodes[collider] = target;
        }
    }

    public void Insert (IReadOnlySet<Collider> colliders)
    {
        foreach (Collider collider in colliders)
        {
            Insert (collider);
        }
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
        node.Colliders.Clear ();
        node.Parent = null;
        node.Next = null;
        node.Child = null;

        return node;
    }

    public void Remove (Collider collider)
    {
        if (_trackColliderNodes)
        {
            if (_colliderNodes.Remove (collider, out SpatialNode? node))
            {
                node.Colliders.Remove (collider);
            }
        }
    }

    public void Remove (IReadOnlySet<Collider> colliders)
    {
        foreach (Collider collider in colliders)
        {
            Remove (collider);
        }
    }

    private SpatialNode? QueryNode (BoundingBox2D bounds)
    {
        SpatialNode? current = _root;
        SpatialNode? target = _root;

        while (current != null)
        {
            if (!current.Bounds.Contains (bounds))
            {
                current = current.Next;
                continue;
            }

            target = current;
            current = current.Child;
        }

        return target;
    }

    public void GetPotentialCollisions (SpatialIndex other, HashSet<ColliderPair> potentialCollisions)
    {
        SpatialNode? current = _root;

        while (current != null)
        {
            SpatialNode? otherNode = other.QueryNode (current.Bounds);

            if (otherNode != null)
            {
                SpatialNode? ancestor = otherNode.Parent;

                while (ancestor != null)
                {
                    GetPotentialCollisions (current.Colliders, ancestor.Colliders, potentialCollisions);

                    ancestor = ancestor.Parent;
                }

                GetPotentialCollisions (current.Colliders, otherNode.Colliders, potentialCollisions);

                SpatialNode? childNode = otherNode.Child;

                while (childNode != null && childNode != otherNode)
                {
                    GetPotentialCollisions (current.Colliders, childNode.Colliders, potentialCollisions);

                    if (childNode.Child != null)
                    {
                        childNode = childNode.Child;
                        continue;
                    }

                    while (childNode != null && childNode != otherNode)
                    {
                        if (childNode.Next != null)
                        {
                            childNode = childNode.Next;
                            break;
                        }

                        childNode = childNode.Parent;
                    }
                }
            }

            current = FindNextNode (current);
        }
    }

    public void GetPotentialCollisions (HashSet<ColliderPair> potentialCollisions)
    {
        SpatialNode? current = _root;

        while (current != null)
        {
            SpatialNode? ancestor = current.Parent;

            while (ancestor != null)
            {
                GetPotentialCollisions (current.Colliders, ancestor.Colliders, potentialCollisions);

                ancestor = ancestor.Parent;
            }

            GetPotentialCollisions (current.Colliders, potentialCollisions);

            current = FindNextNode (current);
        }
    }

    private static void GetPotentialCollisions (List<Collider> self, List<Collider> other, HashSet<ColliderPair> potentialCollisions)
    {
        foreach (Collider selfCollider in self)
        {
            foreach (Collider otherCollider in other)
            {
                if (selfCollider.PhysicsBody == otherCollider.PhysicsBody)
                {
                    continue;
                }

                if (selfCollider.Bounds.Intersects (otherCollider.Bounds))
                {
                    potentialCollisions.Add (new (selfCollider, otherCollider));
                }
            }
        }
    }

    private static void GetPotentialCollisions (List<Collider> self, HashSet<ColliderPair> potentialCollisions)
    {
        for (int i = 0; i < self.Count; i++)
        {
            Collider collider1 = self[i];

            for (int j = i + 1; j < self.Count; j++)
            {
                Collider collider2 = self[j];

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
    }

    private static SpatialNode? FindNextNode (SpatialNode? current)
    {
        if (current == null)
        {
            return null;
        }

        if (current.Child != null)
        {
            return current.Child;
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

        return current;
    }
}
