using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class BoxCollider : Collider
{
    protected Vector2 _size = Vector2.Zero;

    public Vector2 Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                SetDirty (DirtyFlags.All);
            }
        }
    }

    protected override BoundingBox2D CalculateBounds ()
    {
        Vector2 halfSize = Size / 2f;

        float cos = float.Abs (float.Cos (Rotation));
        float sin = float.Abs (float.Sin (Rotation));

        float halfWidth = cos * halfSize.X + sin * halfSize.Y;
        float halfHeight = sin * halfSize.X + cos * halfSize.Y;

        Vector2 center = Position + Vector2.Rotate (Offset, Rotation);
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
            points[i] = Position + Vector2.Rotate (points[i], Rotation) + Vector2.Rotate (Offset, Rotation);
        }
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
