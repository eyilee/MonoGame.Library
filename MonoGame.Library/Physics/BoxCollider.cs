using Microsoft.Xna.Framework;
using MonoGame.Library.Utilities;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class BoxCollider : Collider
{
    public Vector2 Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                SetDirty (DirtyFlag.All);
            }
        }
    }

    protected Vector2 _size = Vector2.Zero;

    protected override BoundingBox2D CalculateBounds ()
    {
        Vector2 halfSize = Size / 2f;

        float cos = float.Abs (float.Cos (Rotation));
        float sin = float.Abs (float.Sin (Rotation));

        float halfWidth = cos * halfSize.X + sin * halfSize.Y;
        float halfHeight = sin * halfSize.X + cos * halfSize.Y;

        Vector2 center = Position + Offset;
        Vector2 extents = new (halfWidth, halfHeight);

        return new BoundingBox2D (center - extents, center + extents);
    }

    protected override void CalculatePoints (List<Vector2> points)
    {
        points.Clear ();

        Vector2 halfSize = Size / 2f;

        points.AddRange ([
            new (-halfSize.X, -halfSize.Y),
            new (halfSize.X, -halfSize.Y),
            new (halfSize.X, halfSize.Y),
            new (-halfSize.X, halfSize.Y)
            ]);

        for (int i = 0; i < 4; i++)
        {
            points[i] = Position + Vector2.Rotate (points[i], Rotation) + Offset;
        }
    }

    public override bool Intersects (Collider other)
    {
        return other.Intersects (this);
    }

    public override bool Intersects (BoxCollider other)
    {
        bool hasValidAxis = false;

        for (int i = 0; i < 4; i++)
        {
            Vector2 p1;
            Vector2 p2;

            if (i < 2)
            {
                p1 = Points[i];
                p2 = Points[(i + 1) % 4];
            }
            else
            {
                p1 = other.Points[i];
                p2 = other.Points[(i + 1) % 4];
            }

            Vector2 edge = p2 - p1;

            if (edge.LengthSquared () <= float.Epsilon)
            {
                continue;
            }

            hasValidAxis = true;

            Vector2 axis = Vector2.Normalize (edge.Perpendicular ());

            float minA = float.MaxValue;
            float maxA = float.MinValue;

            foreach (Vector2 point in Points)
            {
                float projection = Vector2.Dot (point, axis);
                minA = float.Min (minA, projection);
                maxA = float.Max (maxA, projection);
            }

            float minB = float.MaxValue;
            float maxB = float.MinValue;

            foreach (Vector2 point in other.Points)
            {
                float projection = Vector2.Dot (point, axis);
                minB = float.Min (minB, projection);
                maxB = float.Max (maxB, projection);
            }

            float overlap = float.Min (maxA, maxB) - float.Max (minA, minB);
            if (overlap < 0)
            {
                return false;
            }
        }

        return hasValidAxis;
    }

    public override bool Intersects (CircleCollider other)
    {
        Vector2 center = other.Position + other.Offset;

        foreach (Vector2 point in Points)
        {
            if (Vector2.DistanceSquared (point, center) <= other.Radius * other.Radius)
            {
                return true;
            }
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
        bool hasValidAxis = false;

        Vector2 normal = Vector2.Zero;
        float penetration = float.MaxValue;

        for (int i = 0; i < 4; i++)
        {
            Vector2 p1;
            Vector2 p2;

            if (i < 2)
            {
                p1 = Points[i];
                p2 = Points[(i + 1) % 4];
            }
            else
            {
                p1 = other.Points[i];
                p2 = other.Points[(i + 1) % 4];
            }

            Vector2 edge = p2 - p1;

            if (edge.LengthSquared () <= float.Epsilon)
            {
                continue;
            }

            hasValidAxis = true;

            Vector2 axis = Vector2.Normalize (edge.Perpendicular ());

            if (Vector2.Dot ((Position + Offset) - (other.Position + other.Offset), axis) > 0f)
            {
                axis *= -1f;
            }

            float minA = float.MaxValue;
            float maxA = float.MinValue;

            foreach (Vector2 point in Points)
            {
                float projection = Vector2.Dot (point, axis);
                minA = float.Min (minA, projection);
                maxA = float.Max (maxA, projection);
            }

            float minB = float.MaxValue;
            float maxB = float.MinValue;

            foreach (Vector2 point in other.Points)
            {
                float projection = Vector2.Dot (point, axis);
                minB = float.Min (minB, projection);
                maxB = float.Max (maxB, projection);
            }

            float overlap = float.Min (maxA, maxB) - float.Max (minA, minB);

            if (overlap < 0)
            {
                contact = default;
                return false;
            }

            if (overlap < penetration)
            {
                normal = axis;
                penetration = overlap;
            }
        }

        contact = new Contact
        {
            Normal = normal,
            Penetration = penetration
        };

        return hasValidAxis;
    }

    public override bool TryGetContact (CircleCollider other, out Contact contact)
    {
        Vector2 center = other.Points[0];

        Vector2 closest = default;
        Vector2 closestEdgeNormal = default;

        float minDistanceSquared = float.MaxValue;

        bool hasPositive = false;
        bool hasNegative = false;

        for (int i = 0; i < 4; i++)
        {
            Vector2 p1 = Points[i];
            Vector2 p2 = Points[(i + 1) % 4];

            Vector2 edge = p2 - p1;
            float lengthSquared = edge.LengthSquared ();

            if (lengthSquared <= float.Epsilon)
            {
                continue;
            }

            Vector2 toCenter = center - p1;
            float cross = edge.Cross (toCenter);
            hasPositive |= cross > 0f;
            hasNegative |= cross < 0f;

            float t = float.Clamp (Vector2.Dot (center - p1, edge) / lengthSquared, 0f, 1f);
            Vector2 point = p1 + edge * t;
            float distanceSquared = Vector2.DistanceSquared (point, center);

            if (distanceSquared < minDistanceSquared)
            {
                closest = point;
                closestEdgeNormal = Vector2.Normalize (edge.Perpendicular ());
                minDistanceSquared = distanceSquared;

                if (Vector2.Dot ((Position + Offset) - center, closestEdgeNormal) > 0f)
                {
                    closestEdgeNormal *= -1f;
                }
            }
        }

        bool centerInsideBox = !(hasPositive && hasNegative);

        if (!centerInsideBox && minDistanceSquared > other.Radius * other.Radius)
        {
            contact = default;
            return false;
        }

        Vector2 normal;

        if (minDistanceSquared > float.Epsilon)
        {
            normal = Vector2.Normalize (center - closest);
        }
        else
        {
            normal = closestEdgeNormal;
        }

        float distance = Vector2.Distance (closest, center);

        contact = new Contact
        {
            Normal = normal,
            Penetration = centerInsideBox ? other.Radius + distance : other.Radius - distance
        };

        return true;
    }

    public override bool TryGetContact (PolygonCollider other, out Contact contact)
    {
        throw new System.NotImplementedException ();
    }
}
