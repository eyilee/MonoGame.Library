using Microsoft.Xna.Framework;
using MonoGame.Library.Utilities;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class Physics2D
{
    public const float Epsilon = 1e-6f;

    public const float EpsilonSquared = Epsilon * Epsilon;

    public static bool Intersects (BoxCollider self, BoxCollider other)
    {
        return Intersects (new Polygon (self.Points), new Polygon (other.Points));
    }

    public static bool Intersects (BoxCollider self, CircleCollider other)
    {
        return Intersects (new Polygon (self.Points), new Circle (other.Points[0], other.Radius));
    }

    public static bool Intersects (BoxCollider self, PolygonCollider other)
    {
        return Intersects (new Polygon (self.Points), new Polygon (other.Points));
    }

    public static bool Intersects (CircleCollider self, BoxCollider other)
    {
        return Intersects (new Circle (self.Points[0], self.Radius), new Polygon (other.Points));
    }

    public static bool Intersects (CircleCollider self, CircleCollider other)
    {
        Vector2 p1 = self.Position + self.Offset;
        Vector2 p2 = other.Position + other.Offset;

        if (Vector2.DistanceSquared (p1, p2) <= (self.Radius + other.Radius) * (self.Radius + other.Radius))
        {
            return true;
        }

        return false;
    }

    public static bool Intersects (CircleCollider self, PolygonCollider other)
    {
        return Intersects (new Circle (self.Points[0], self.Radius), new Polygon (other.Points));
    }

    public static bool Intersects (PolygonCollider self, BoxCollider other)
    {
        return Intersects (new Polygon (self.Points), new Polygon (other.Points));
    }

    public static bool Intersects (PolygonCollider self, CircleCollider other)
    {
        return Intersects (new Polygon (self.Points), new Circle (other.Points[0], other.Radius));
    }

    public static bool Intersects (PolygonCollider self, PolygonCollider other)
    {
        return Intersects (new Polygon (self.Points), new Polygon (other.Points));
    }

    public static bool Intersects (in ISupportable self, in ISupportable other)
    {
        return Intersects (self, other, out _);
    }

    private static bool Intersects (in ISupportable self, in ISupportable other, out List<Vector2> simplex)
    {
        Vector2 direction = self.Position - other.Position;

        if (direction.LengthSquared () < EpsilonSquared)
        {
            direction = Vector2.UnitX;
        }

        simplex = [self.Support (direction) - other.Support (-direction)];
        direction = -simplex[0];

        while (true)
        {
            Vector2 point = self.Support (direction) - other.Support (-direction);

            if (Vector2.Dot (point, direction) <= 0)
            {
                return false;
            }

            simplex.Add (point);

            if (simplex.Count == 2)
            {
                Vector2 a = simplex[1];
                Vector2 b = simplex[0];
                Vector2 ab = b - a;
                Vector2 ao = -a;

                direction = TripleProduct (ab, ao, ab);
            }
            else if (simplex.Count == 3)
            {
                Vector2 a = simplex[2];
                Vector2 b = simplex[1];
                Vector2 c = simplex[0];
                Vector2 ab = b - a;
                Vector2 ac = c - a;
                Vector2 ao = -a;
                Vector2 abPerp = TripleProduct (ac, ab, ab);
                Vector2 acPerp = TripleProduct (ab, ac, ac);

                if (Vector2.Dot (abPerp, ao) > 0)
                {
                    simplex.RemoveAt (0);
                    direction = abPerp;
                }
                else if (Vector2.Dot (acPerp, ao) > 0)
                {
                    simplex.RemoveAt (1);
                    direction = acPerp;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    private static Vector2 TripleProduct (Vector2 v1, Vector2 v2, Vector2 v3)
    {
        float cross = v1.X * v2.Y - v1.Y * v2.X;

        return new Vector2 (-cross * v3.Y, cross * v3.X);
    }

    public static bool TryGetContact (BoxCollider self, BoxCollider other, out Contact contact)
    {
        return TryGetContact (new Polygon (self.Points), new Polygon (other.Points), out contact);
    }

    public static bool TryGetContact (BoxCollider self, CircleCollider other, out Contact contact)
    {
        return TryGetContact (new Polygon (self.Points), new Circle (other.Points[0], other.Radius), out contact);
    }

    public static bool TryGetContact (BoxCollider self, PolygonCollider other, out Contact contact)
    {
        return TryGetContact (new Polygon (self.Points), new Polygon (other.Points), out contact);
    }

    public static bool TryGetContact (CircleCollider self, BoxCollider other, out Contact contact)
    {
        return TryGetContact (new Circle (self.Points[0], self.Radius), new Polygon (other.Points), out contact);
    }

    public static bool TryGetContact (CircleCollider self, CircleCollider other, out Contact contact)
    {
        contact = default;

        Vector2 p1 = self.Position + self.Offset;
        Vector2 p2 = other.Position + other.Offset;
        float distance = Vector2.Distance (p1, p2);
        float minDistance = self.Radius + other.Radius;

        if (distance >= minDistance)
        {
            return false;
        }

        Vector2 normal = distance > Epsilon ? (p2 - p1) / distance : Vector2.UnitX;

        contact = new Contact
        {
            Normal = normal,
            Penetration = minDistance - distance
        };

        return true;
    }

    public static bool TryGetContact (CircleCollider self, PolygonCollider other, out Contact contact)
    {
        return TryGetContact (new Circle (self.Points[0], self.Radius), new Polygon (other.Points), out contact);
    }

    public static bool TryGetContact (PolygonCollider self, BoxCollider other, out Contact contact)
    {
        return TryGetContact (new Polygon (self.Points), new Polygon (other.Points), out contact);
    }

    public static bool TryGetContact (PolygonCollider self, CircleCollider other, out Contact contact)
    {
        return TryGetContact (new Polygon (self.Points), new Circle (other.Points[0], other.Radius), out contact);
    }

    public static bool TryGetContact (PolygonCollider self, PolygonCollider other, out Contact contact)
    {
        return TryGetContact (new Polygon (self.Points), new Polygon (other.Points), out contact);
    }

    private static bool TryGetContact (in ISupportable self, in ISupportable other, out Contact contact)
    {
        if (!Intersects (self, other, out List<Vector2> simplex))
        {
            contact = default;

            return false;
        }

        return TryGetContact (self, other, simplex, out contact);
    }

    private static bool TryGetContact (in ISupportable self, in ISupportable other, List<Vector2> simplex, out Contact contact)
    {
        if (simplex.Count != 3)
        {
            contact = default;

            return false;
        }

        if ((simplex[1] - simplex[0]).Cross (simplex[2] - simplex[0]) < 0)
        {
            simplex.Reverse ();
        }

        for (int iteration = 0; iteration < 32; iteration++)
        {
            int cloestEdgeIndex = -1;
            Vector2 closestNormal = Vector2.Zero;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < simplex.Count; i++)
            {
                Vector2 a = simplex[i];
                Vector2 b = simplex[(i + 1) % simplex.Count];
                Vector2 edge = b - a;

                if (edge.LengthSquared () < EpsilonSquared)
                {
                    continue;
                }

                Vector2 normal = new (edge.Y, -edge.X);
                normal.Normalize ();

                float distance = Vector2.Dot (normal, a);

                if (distance < 0)
                {
                    normal = -normal;
                    distance = -distance;
                }

                if (distance <= closestDistance)
                {
                    cloestEdgeIndex = i;
                    closestNormal = normal;
                    closestDistance = distance;
                }
            }

            if (cloestEdgeIndex == -1)
            {
                contact = default;

                return false;
            }

            Vector2 point = self.Support (closestNormal) - other.Support (-closestNormal);
            float pointDistance = Vector2.Dot (closestNormal, point);

            if (pointDistance - closestDistance < Epsilon)
            {
                contact = new Contact
                {
                    Normal = closestNormal,
                    Penetration = closestDistance
                };

                return true;
            }

            simplex.Insert (cloestEdgeIndex + 1, point);
        }

        contact = default;

        return false;
    }
}
