using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public abstract class Collider
{
    [Flags]
    protected enum DirtyFlags
    {
        None = 0,
        Bounds = 1,
        Points = 2,
        All = Bounds | Points
    }

    public PhysicsBody PhysicsBody { get; internal set; } = null!;

    public Transform Transform => PhysicsBody.Transform;

    public Vector2 Position => Transform.Position;

    public float Rotation => Transform.Rotation;

    public Vector2 Offset
    {
        get => _offset;
        set
        {
            if (_offset != value)
            {
                _offset = value;
                SetDirty (DirtyFlags.All);
            }
        }
    }

    public BoundingBox2D Bounds
    {
        get
        {
            if (IsDirty (DirtyFlags.Bounds))
            {
                _bounds = CalculateBounds ();
                ClearDirty (DirtyFlags.Bounds);
            }

            return _bounds;
        }
    }

    public List<Vector2> Points
    {
        get
        {
            if (IsDirty (DirtyFlags.Points))
            {
                CalculatePoints (_points);
                ClearDirty (DirtyFlags.Points);
            }

            return _points;
        }
    }

    private Vector2 _offset;

    private BoundingBox2D _bounds;

    private readonly List<Vector2> _points = [];

    private DirtyFlags _dirtyFlags = DirtyFlags.All;

    protected abstract BoundingBox2D CalculateBounds ();

    protected abstract void CalculatePoints (List<Vector2> points);

    protected bool IsDirty (DirtyFlags dirtyFlag) => _dirtyFlags.HasFlag (dirtyFlag);

    protected void SetDirty (DirtyFlags dirtyFlag) => _dirtyFlags |= dirtyFlag;

    protected void ClearDirty (DirtyFlags dirtyFlag) => _dirtyFlags &= ~dirtyFlag;

    public virtual void OnTransformChanged (Transform.ChangedFlags changedFlags)
    {
        if (changedFlags.HasFlag (Transform.ChangedFlags.Position) || changedFlags.HasFlag (Transform.ChangedFlags.Rotation))
        {
            SetDirty (DirtyFlags.All);
        }
    }

    public abstract bool Intersects (Collider other);

    public abstract bool Intersects (BoxCollider other);

    public abstract bool Intersects (CircleCollider other);

    public abstract bool Intersects (PolygonCollider other);

    public abstract bool TryGetContacts (Collider other, out Contact selfContact, out Contact otherContact);

    public abstract bool TryGetContacts (BoxCollider other, out Contact selfContact, out Contact otherContact);

    public abstract bool TryGetContacts (CircleCollider other, out Contact selfContact, out Contact otherContact);

    public abstract bool TryGetContacts (PolygonCollider other, out Contact selfContact, out Contact otherContact);
}
