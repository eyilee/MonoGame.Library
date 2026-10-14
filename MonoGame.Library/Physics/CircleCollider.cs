using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class CircleCollider : Collider
{
    protected float _radius = 0f;

    public float Radius
    {
        get => _radius;
        set
        {
            if (_radius != value)
            {
                _radius = value;
                SetDirty (DirtyFlags.All);
            }
        }
    }

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

    public override bool TryGetContacts (Collider other, out Contact selfContact, out Contact otherContact) => other.TryGetContacts (this, out otherContact, out selfContact);

    public override bool TryGetContacts (BoxCollider other, out Contact selfContact, out Contact otherContact) => Physics2D.TryGetContacts (this, other, out selfContact, out otherContact);

    public override bool TryGetContacts (CircleCollider other, out Contact selfContact, out Contact otherContact) => Physics2D.TryGetContacts (this, other, out selfContact, out otherContact);

    public override bool TryGetContacts (PolygonCollider other, out Contact selfContact, out Contact otherContact) => Physics2D.TryGetContacts (this, other, out selfContact, out otherContact);
}
