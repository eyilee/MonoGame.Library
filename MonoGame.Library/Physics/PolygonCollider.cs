using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class PolygonCollider : Collider
{
    public IReadOnlyList<Vector2> Vertices => _vertices;

    protected List<Vector2> _vertices = [];

    public void SetVertices (List<Vector2> vertices)
    {
        _vertices.Clear ();
        _vertices.Capacity = vertices.Count;

        foreach (Vector2 vertex in vertices)
        {
            _vertices.Add (vertex);
        }

        SetDirty (DirtyFlag.All);
    }

    protected override BoundingBox2D CalculateBounds ()
    {
        return CalculateBounds (Position, Rotation);
    }

    protected override BoundingBox2D CalculateBounds (Vector2 position, float rotation)
    {
        List<Vector2> points = CalculatePoints (position, rotation);

        Vector2 min = points[0];
        Vector2 max = min;

        foreach (Vector2 point in points)
        {
            min.X = float.Min (min.X, point.X);
            min.Y = float.Min (min.Y, point.Y);
            max.X = float.Max (max.X, point.X);
            max.Y = float.Max (max.Y, point.Y);
        }

        return new BoundingBox2D (min, max);
    }

    protected override List<Vector2> CalculatePoints ()
    {
        return CalculatePoints (Position, Rotation);
    }

    protected override List<Vector2> CalculatePoints (Vector2 position, float rotation)
    {
        List<Vector2> points = new (_vertices.Count);

        foreach (Vector2 vertex in _vertices)
        {
            points.Add (Position + Vector2.Rotate (vertex, Rotation) + Offset);
        }

        return points;
    }

    public override bool Intersects (Collider other)
    {
        return other.Intersects (this);
    }

    public override bool Intersects (BoxCollider other)
    {
        throw new System.NotImplementedException ();
    }

    public override bool Intersects (CircleCollider other)
    {
        throw new System.NotImplementedException ();
    }

    public override bool Intersects (PolygonCollider other)
    {
        throw new System.NotImplementedException ();
    }

    public override bool TryGetContact (Collider other, out Contact contact)
    {
        throw new System.NotImplementedException ();
    }

    public override bool TryGetContact (BoxCollider other, out Contact contact)
    {
        throw new System.NotImplementedException ();
    }

    public override bool TryGetContact (CircleCollider other, out Contact contact)
    {
        throw new System.NotImplementedException ();
    }

    public override bool TryGetContact (PolygonCollider other, out Contact contact)
    {
        throw new System.NotImplementedException ();
    }
}
