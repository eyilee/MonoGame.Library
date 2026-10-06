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
