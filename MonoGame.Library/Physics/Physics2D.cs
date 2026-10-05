using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class Physics2D
{
    public const float Epsilon = 1e-6f;

    public const float EpsilonSquared = Epsilon * Epsilon;

    public static bool Intersects (in ISupportable self, in ISupportable other)
    {
        return Intersects (self, other, out _);
    }

    public static bool Intersects (in ISupportable self, in ISupportable other, out List<Vector2> simplex)
    {
        Vector2 direction = self.Center - other.Center;

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

    public static bool TryGetContact (in ISupportable self, in ISupportable other, out Contact contact)
    {
        if (!Intersects (self, other, out List<Vector2> simplex))
        {
            contact = default;

            return false;
        }

        var result = TryGetContact (self, other, simplex, out contact);
        Console.WriteLine (result);
        Console.WriteLine (contact.Penetration);
        Console.WriteLine (contact.Normal);
        return result;
    }

    public static bool TryGetContact (in ISupportable self, in ISupportable other, List<Vector2> simplex, out Contact contact)
    {
        if (simplex.Count != 3)
        {
            contact = default;
            return false;
        }

        List<float> distances = [];

        for (int iteration = 0; iteration < 32; iteration++)
        {
            int cloestEdgeIndex = -1;
            Vector2 closestNormal = Vector2.Zero;
            float closestDistance = float.MaxValue;

            distances.Clear ();

            for (int i = 0; i < simplex.Count; i++)
            {
                Vector2 a = simplex[i];
                Vector2 b = simplex[(i + 1) % simplex.Count];
                Vector2 edge = b - a;

                if (edge.LengthSquared () < EpsilonSquared)
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

                distances.Add (distance);

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

            bool duplicate = false;

            for (int i = 0; i < simplex.Count; i++)
            {
                if (Vector2.DistanceSquared (point, simplex[i]) <= Epsilon)
                {
                    duplicate = true;
                }
            }

            float pointDistance = Vector2.Dot (closestNormal, point);

            if (pointDistance - closestDistance < Epsilon || duplicate)
            {
                contact = new Contact
                {
                    Normal = closestNormal,
                    Penetration = closestDistance
                };

                return true;
            }

            if (simplex.Contains (point))
            {
                Console.WriteLine ("");
            }

            simplex.Insert (cloestEdgeIndex + 1, point);
        }

        contact = default;

        return false;
    }
}
