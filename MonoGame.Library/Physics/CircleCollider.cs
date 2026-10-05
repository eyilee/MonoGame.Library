using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class CircleCollider : Collider
{
    public float Radius
    {
        get => _radius;
        set
        {
            if (_radius != value)
            {
                _radius = value;
                SetDirty (DirtyFlag.All);
            }
        }
    }

    protected float _radius = 0f;

    protected override BoundingBox2D CalculateBounds ()
    {
        Vector2 center = Position + Offset;
        Vector2 extents = new (Radius, Radius);

        return new BoundingBox2D (center - extents, center + extents);
    }

    protected override void CalculatePoints (List<Vector2> points)
    {
        points.Clear ();
        points.Add (Position + Offset);
    }

    public override bool Intersects (Collider other)
    {
        return other.Intersects (this);
    }

    public override bool Intersects (BoxCollider other)
    {
        Vector2 center = Position + Offset;

        foreach (Vector2 point in other.Points)
        {
            if (Vector2.DistanceSquared (point, center) <= Radius * Radius)
            {
                return true;
            }
        }

        return false;
    }

    public override bool Intersects (CircleCollider other)
    {
        Vector2 p1 = Position + Offset;
        Vector2 p2 = other.Position + other.Offset;

        if (Vector2.DistanceSquared (p1, p2) <= (Radius + other.Radius) * (Radius + other.Radius))
        {
            return true;
        }

        return false;
    }

    public override bool Intersects (PolygonCollider other)
    {
        throw new System.NotImplementedException ();
    }

    public override bool TryGetContact (Collider other, out Contact contact)
    {
        return other.TryGetContact (this, out contact);
    }

    public override bool TryGetContact (BoxCollider other, out Contact contact)
    {
        if (!other.TryGetContact (this, out contact))
        {
            return false;
        }

        contact.Normal *= -1f;

        return true;
    }

    public override bool TryGetContact (CircleCollider other, out Contact contact)
    {
        contact = default;

        Vector2 centerA = Position + Offset;
        Vector2 centerB = other.Position + other.Offset;
        float distance = Vector2.Distance (centerA, centerB);
        float minDistance = Radius + other.Radius;

        if (distance >= minDistance)
        {
            return false;
        }

        Vector2 normal = distance > float.Epsilon ? (centerB - centerA) / distance : Vector2.UnitX;

        contact = new Contact
        {
            Normal = normal,
            Penetration = minDistance - distance
        };

        return true;
    }

    public override bool TryGetContact (PolygonCollider other, out Contact contact)
    {
        throw new System.NotImplementedException ();
    }
}
