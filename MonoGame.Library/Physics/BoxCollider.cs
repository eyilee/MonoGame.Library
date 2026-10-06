using Microsoft.Xna.Framework;
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
