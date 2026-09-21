using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public abstract class Collider
{
    [Flags]
    protected enum DirtyFlag
    {
        None = 0,
        Bounds = 1,
        Points = 2,
        All = Bounds | Points
    }

    public Vector2 Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                SetDirty (DirtyFlag.All);
            }
        }
    }

    public float Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                SetDirty (DirtyFlag.All);
            }
        }
    }

    public Vector2 Offset
    {
        get => _offset;
        set
        {
            if (_offset != value)
            {
                _offset = value;
                SetDirty (DirtyFlag.All);
            }
        }
    }

    public BoundingBox2D Bounds
    {
        get
        {
            if (IsDirty (DirtyFlag.Bounds))
            {
                _bounds = CalculateBounds ();
                ClearDirty (DirtyFlag.Bounds);
            }

            return _bounds;
        }
    }

    public List<Vector2> Points
    {
        get
        {
            if (IsDirty (DirtyFlag.Points))
            {
                _points = CalculatePoints ();
                ClearDirty (DirtyFlag.Points);
            }

            return _points;
        }
    }

    private Vector2 _position;

    private float _rotation;

    private Vector2 _offset;

    private BoundingBox2D _bounds;

    private List<Vector2> _points = [];

    private DirtyFlag _dirtyFlags = DirtyFlag.All;

    protected abstract BoundingBox2D CalculateBounds ();

    protected abstract List<Vector2> CalculatePoints ();

    protected bool IsDirty (DirtyFlag dirtyFlag) => _dirtyFlags.HasFlag (dirtyFlag);

    protected void SetDirty (DirtyFlag dirtyFlag) => _dirtyFlags |= dirtyFlag;

    protected void ClearDirty (DirtyFlag dirtyFlag) => _dirtyFlags &= ~dirtyFlag;

    public abstract bool Intersects (Collider other);

    public abstract bool Intersects (BoxCollider other);

    public abstract bool Intersects (CircleCollider other);

    public abstract bool TryGetContact (Collider other, out Contact contact);

    public abstract bool TryGetContact (BoxCollider other, out Contact contact);

    public abstract bool TryGetContact (CircleCollider other, out Contact contact);
}
