using Microsoft.Xna.Framework;
using MonoGame.Library.Utilities;
using System;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public struct SupportPoint (Vector2 self, Vector2 other)
{
    public Vector2 Self { get; set; } = self;

    public Vector2 Other { get; set; } = other;

    public readonly Vector2 Value => Self - Other;
}

public class Physics2D
{
    public const float Epsilon = 1e-6f;

    public const float EpsilonSquared = Epsilon * Epsilon;

    public static bool Intersects (BoxCollider self, BoxCollider other) => Intersects (new Polygon (self.Points), new Polygon (other.Points));

    public static bool Intersects (BoxCollider self, CircleCollider other) => Intersects (new Polygon (self.Points), new Circle (other.Points[0], other.Radius));

    public static bool Intersects (BoxCollider self, PolygonCollider other) => Intersects (new Polygon (self.Points), new Polygon (other.Points));

    public static bool Intersects (CircleCollider self, BoxCollider other) => Intersects (new Circle (self.Points[0], self.Radius), new Polygon (other.Points));

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

    public static bool Intersects (CircleCollider self, PolygonCollider other) => Intersects (new Circle (self.Points[0], self.Radius), new Polygon (other.Points));

    public static bool Intersects (PolygonCollider self, BoxCollider other) => Intersects (new Polygon (self.Points), new Polygon (other.Points));

    public static bool Intersects (PolygonCollider self, CircleCollider other) => Intersects (new Polygon (self.Points), new Circle (other.Points[0], other.Radius));

    public static bool Intersects (PolygonCollider self, PolygonCollider other) => Intersects (new Polygon (self.Points), new Polygon (other.Points));

    public static bool Intersects (in ISupportable self, in ISupportable other) => Intersects (self, other, out _);

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

    public static bool TryGetContacts (BoxCollider self, BoxCollider other, out Contact selfContact, out Contact otherContact) => TryGetContacts (new Polygon (self.Points), new Polygon (other.Points), out selfContact, out otherContact);

    public static bool TryGetContacts (BoxCollider self, CircleCollider other, out Contact selfContact, out Contact otherContact) => TryGetContacts (new Polygon (self.Points), new Circle (other.Points[0], other.Radius), out selfContact, out otherContact);

    public static bool TryGetContacts (BoxCollider self, PolygonCollider other, out Contact selfContact, out Contact otherContact) => TryGetContacts (new Polygon (self.Points), new Polygon (other.Points), out selfContact, out otherContact);

    public static bool TryGetContacts (CircleCollider self, BoxCollider other, out Contact selfContact, out Contact otherContact) => TryGetContacts (new Circle (self.Points[0], self.Radius), new Polygon (other.Points), out selfContact, out otherContact);

    public static bool TryGetContacts (CircleCollider self, CircleCollider other, out Contact selfContact, out Contact otherContact)
    {
        Vector2 p1 = self.Position + self.Offset;
        Vector2 p2 = other.Position + other.Offset;
        float distance = Vector2.Distance (p1, p2);
        float minDistance = self.Radius + other.Radius;

        if (distance >= minDistance)
        {
            selfContact = default;
            otherContact = default;

            return false;
        }

        Vector2 normal = distance > Epsilon ? (p2 - p1) / distance : Vector2.UnitX;

        selfContact = new Contact
        {
            Normal = normal,
            Penetration = minDistance - distance
        };

        otherContact = new Contact
        {
            Normal = -normal,
            Penetration = minDistance - distance
        };

        return true;
    }

    public static bool TryGetContacts (CircleCollider self, PolygonCollider other, out Contact selfContact, out Contact otherContact) => TryGetContacts (new Circle (self.Points[0], self.Radius), new Polygon (other.Points), out selfContact, out otherContact);

    public static bool TryGetContacts (PolygonCollider self, BoxCollider other, out Contact selfContact, out Contact otherContact) => TryGetContacts (new Polygon (self.Points), new Polygon (other.Points), out selfContact, out otherContact);

    public static bool TryGetContacts (PolygonCollider self, CircleCollider other, out Contact selfContact, out Contact otherContact) => TryGetContacts (new Polygon (self.Points), new Circle (other.Points[0], other.Radius), out selfContact, out otherContact);

    public static bool TryGetContacts (PolygonCollider self, PolygonCollider other, out Contact selfContact, out Contact otherContact) => TryGetContacts (new Polygon (self.Points), new Polygon (other.Points), out selfContact, out otherContact);

    private static bool TryGetContacts (in ISupportable self, in ISupportable other, out Contact selfContact, out Contact otherContact)
    {
        if (!Intersects (self, other, out List<Vector2> simplex))
        {
            selfContact = default;
            otherContact = default;

            return false;
        }

        if (!TryGetPenetration (self, other, simplex, out Vector2 normal, out float penetration))
        {
            selfContact = default;
            otherContact = default;

            return false;
        }

        self.GetContacts (other, normal, penetration, out selfContact, out otherContact);

        return true;
    }

    private static bool TryGetPenetration (in ISupportable self, in ISupportable other, List<Vector2> simplex, out Vector2 normal, out float penetration)
    {
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

                Vector2 edgeNormal = new (edge.Y, -edge.X);
                edgeNormal.Normalize ();

                float distance = Vector2.Dot (edgeNormal, a);

                if (distance < 0)
                {
                    edgeNormal = -edgeNormal;
                    distance = -distance;
                }

                if (distance <= closestDistance)
                {
                    cloestEdgeIndex = i;
                    closestNormal = edgeNormal;
                    closestDistance = distance;
                }
            }

            if (cloestEdgeIndex == -1)
            {
                break;
            }

            Vector2 point = self.Support (closestNormal) - other.Support (-closestNormal);
            float pointDistance = Vector2.Dot (closestNormal, point);

            if (pointDistance - closestDistance < Epsilon)
            {
                normal = closestNormal;
                penetration = closestDistance;

                return true;
            }

            simplex.Insert (cloestEdgeIndex + 1, point);
        }

        normal = default;
        penetration = default;

        return false;
    }

    public static void GetContacts (in Circle self, in Circle other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact)
    {
        selfContact = new Contact
        {
            Point = self.Position + self.Radius * normal,
            Normal = normal,
            Penetration = penetration
        };

        otherContact = new Contact
        {
            Point = other.Position - other.Radius * normal,
            Normal = -normal,
            Penetration = penetration
        };
    }

    public static void GetContacts (in Circle self, in Polygon other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact) => GetContacts (other, self, -normal, penetration, out otherContact, out selfContact);

    public static void GetContacts (in Polygon self, in Circle other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact)
    {
        otherContact = new Contact
        {
            Point = other.Position - other.Radius * normal,
            Normal = -normal,
            Penetration = penetration
        };

        Vector2[] referenceEdge = GetReferenceEdge (self, normal);

        selfContact = new Contact
        {
            Point = ProjectPointOnEdge (otherContact.Point, referenceEdge[0], referenceEdge[1]),
            Normal = normal,
            Penetration = penetration
        };
    }

    public static void GetContacts (in Polygon self, in Polygon other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact)
    {
        Vector2[] referenceEdge = GetReferenceEdge (self, normal);
        Vector2[] incidentEdge = GetIncidentEdge (other, normal);

        incidentEdge = ClipEdge (incidentEdge, referenceEdge[0], referenceEdge[1]);
        incidentEdge = ClipEdge (incidentEdge, referenceEdge[1], referenceEdge[0]);

        float distanceStart = Vector2.Dot (incidentEdge[0] - referenceEdge[0], normal);
        float distanceEnd = Vector2.Dot (incidentEdge[1] - referenceEdge[0], normal);

        otherContact = new Contact
        {
            Point = (distanceStart <= distanceEnd) ? incidentEdge[0] : incidentEdge[1],
            Normal = -normal,
            Penetration = penetration
        };

        selfContact = new Contact
        {
            Point = ProjectPointOnEdge (otherContact.Point, referenceEdge[0], referenceEdge[1]),
            Normal = normal,
            Penetration = penetration
        };
    }

    private static Vector2[] ClipEdge (Vector2[] incidentEdge, Vector2 start, Vector2 end)
    {
        Vector2 edge = end - start;

        float lengthSquared = edge.LengthSquared ();
        if (lengthSquared < EpsilonSquared)
        {
            return incidentEdge;
        }

        edge.Normalize ();

        float distanceStart = Vector2.Dot (incidentEdge[0] - start, edge);
        float distanceEnd = Vector2.Dot (incidentEdge[1] - start, edge);

        if (distanceStart >= 0 && distanceEnd >= 0)
        {
            return incidentEdge;
        }

        if (distanceStart < 0 && distanceEnd < 0)
        {
            return [];
        }

        float t = distanceStart / (distanceStart - distanceEnd);
        Vector2 intersectionPoint = incidentEdge[0] + t * (incidentEdge[1] - incidentEdge[0]);

        if (distanceStart < 0)
        {
            return [intersectionPoint, incidentEdge[1]];
        }
        else
        {
            return [incidentEdge[0], intersectionPoint];
        }
    }

    private static Vector2[] GetReferenceEdge (in Polygon polygon, Vector2 normal) => FindBestAlignedEdge (polygon, normal);

    private static Vector2[] GetIncidentEdge (in Polygon polygon, Vector2 normal) => FindBestAlignedEdge (polygon, -normal);

    private static Vector2[] FindBestAlignedEdge (in Polygon polygon, Vector2 normal)
    {
        float maxValue = float.MinValue;
        Vector2[] edgeResult = new Vector2[2];

        for (int i = 0; i < polygon.Points.Count; i++)
        {
            Vector2 a = polygon.Points[i];
            Vector2 b = polygon.Points[(i + 1) % polygon.Points.Count];
            Vector2 edge = b - a;

            if (edge.LengthSquared () < EpsilonSquared)
            {
                continue;
            }

            Vector2 edgeNormal = new (edge.Y, -edge.X);
            edgeNormal.Normalize ();

            float value = Vector2.Dot (edgeNormal, normal);

            if (value > maxValue)
            {
                edgeResult[0] = a;
                edgeResult[1] = b;
            }
        }

        return edgeResult;
    }

    private static Vector2 ProjectPointOnEdge (Vector2 point, Vector2 start, Vector2 end)
    {
        Vector2 edge = end - start;
        float lengthSquared = edge.LengthSquared ();

        if (lengthSquared < EpsilonSquared)
        {
            return start;
        }

        float t = Vector2.Dot (point - start, edge) / lengthSquared;
        t = float.Clamp (t, 0f, 1f);

        return start + edge * t;
    }
}
