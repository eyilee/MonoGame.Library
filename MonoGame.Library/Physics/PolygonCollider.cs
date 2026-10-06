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
        _vertices.AddRange (vertices);

        SetDirty (DirtyFlag.All);
    }

    protected override BoundingBox2D CalculateBounds ()
    {
        if (Points.Count == 0)
        {
            return new BoundingBox2D (Vector2.Zero, Vector2.Zero);
        }

        Vector2 min = Points[0];
        Vector2 max = min;

        foreach (Vector2 point in Points)
        {
            min.X = float.Min (min.X, point.X);
            min.Y = float.Min (min.Y, point.Y);
            max.X = float.Max (max.X, point.X);
            max.Y = float.Max (max.Y, point.Y);
        }

        return new BoundingBox2D (min, max);
    }

    protected override void CalculatePoints (List<Vector2> points)
    {
        points.Clear ();

        foreach (Vector2 vertex in _vertices)
        {
            points.Add (Position + Vector2.Rotate (vertex, Rotation) + Offset);
        }
    }

    public override bool Intersects (Collider other) => other.Intersects (this);

    public override bool Intersects (BoxCollider other) => Physics2D.Intersects (this, other);

    public override bool Intersects (CircleCollider other) => Physics2D.Intersects (this, other);

    public override bool Intersects (PolygonCollider other) => Physics2D.Intersects (this, other);

    public override bool TryGetContact (Collider other, out Contact contact)
    {
        if (!other.TryGetContact (this, out contact))
        {
            return false;
        }

        contact.Normal *= -1f;

        return true;
    }

    public override bool TryGetContact (BoxCollider other, out Contact contact) => Physics2D.TryGetContact (this, other, out contact);

    public override bool TryGetContact (CircleCollider other, out Contact contact) => Physics2D.TryGetContact (this, other, out contact);

    public override bool TryGetContact (PolygonCollider other, out Contact contact) => Physics2D.TryGetContact (this, other, out contact);
}
