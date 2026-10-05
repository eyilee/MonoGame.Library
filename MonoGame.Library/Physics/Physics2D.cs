using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class Physics2D
{
    public const float Epsilon = 1e-6f;

    public static bool Intersects (List<Vector2> self, List<Vector2> other)
    {
        Vector2 direction = self[0] - other[0];

        if (direction.LengthSquared () < Epsilon)
        {
            direction = Vector2.UnitX;
        }

        List<Vector2> simplex = [Support (self, other, direction)];

        direction = -simplex[0];

        while (true)
        {
            if (direction.LengthSquared () < Epsilon)
            {
                return true;
            }

            Vector2 point = Support (self, other, direction);

            if (Vector2.Dot (point, direction) <= 0)
            {
                return false;
            }

            simplex.Add (point);

            if (HandleSimplex (simplex, ref direction))
            {
                return true;
            }
        }
    }

    private static Vector2 Support (List<Vector2> self, List<Vector2> other, Vector2 direction)
    {
        return GetFarthestPoint (self, direction) - GetFarthestPoint (other, -direction);
    }

    private static Vector2 GetFarthestPoint (List<Vector2> polygon, Vector2 direction)
    {
        float maxDot = float.NegativeInfinity;
        Vector2 farthestPoint = Vector2.Zero;

        foreach (Vector2 vertex in polygon)
        {
            float dot = Vector2.Dot (vertex, direction);
            if (dot > maxDot)
            {
                maxDot = dot;
                farthestPoint = vertex;
            }
        }

        return farthestPoint;
    }

    private static bool HandleSimplex (List<Vector2> simplex, ref Vector2 direction)
    {
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

        return false;
    }

    private static Vector2 TripleProduct (Vector2 v1, Vector2 v2, Vector2 v3)
    {
        float cross = v1.X * v2.Y - v1.Y * v2.X;

        return new Vector2 (-cross * v3.Y, cross * v3.X);
    }

    public static bool TryGetContact (List<Vector2> self, List<Vector2> other, out Contact contact)
    {
        Vector2 direction = self[0] - other[0];

        if (direction.LengthSquared () < Epsilon)
        {
            direction = Vector2.UnitX;
        }

        List<Vector2> simplex = [Support (self, other, direction)];

        direction = -simplex[0];

        while (true)
        {
            if (direction.LengthSquared () < Epsilon)
            {
                return TryGetContact (self, other, simplex, out contact);
            }

            Vector2 point = Support (self, other, direction);

            if (Vector2.Dot (point, direction) <= 0)
            {
                contact = default;

                return false;
            }

            simplex.Add (point);

            if (HandleSimplex (simplex, ref direction))
            {
                return TryGetContact (self, other, simplex, out contact);
            }
        }
    }

    private static bool TryGetContact (List<Vector2> self, List<Vector2> other, List<Vector2> simplex, out Contact contact)
    {
        while (true)
        {
            int cloestEdgeIndex = -1;
            Vector2 closestNormal = Vector2.Zero;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < simplex.Count; i++)
            {
                Vector2 a = simplex[i];
                Vector2 b = simplex[(i + 1) % simplex.Count];
                Vector2 edge = b - a;

                if (edge.LengthSquared () < Epsilon)
                {
                    continue;
                }

                Vector2 normal = new (-edge.Y, edge.X);
                normal.Normalize ();

                float distance = Vector2.Dot (normal, a);

                if (distance < 0)
                {
                    normal = -normal;
                    distance = -distance;
                }

                if (distance < closestDistance)
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

            Vector2 point = Support (self, other, closestNormal);

            if (simplex.Contains (point))
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
    }
}
