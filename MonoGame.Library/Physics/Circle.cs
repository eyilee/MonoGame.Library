using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

public struct Circle (Vector2 point, float radius) : ISupportable
{
    public Vector2 Point { get; set; } = point;

    public float Radius { get; set; } = radius;

    public readonly Vector2 Position => Point;

    public readonly Vector2 Support (Vector2 direction)
    {
        return Point + Vector2.Normalize (direction) * Radius;
    }

    public readonly void GetContacts (in ISupportable other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact) => other.GetContacts (this, -normal, penetration, out otherContact, out selfContact);

    public readonly void GetContacts (in Circle other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact) => Physics2D.GetContacts (this, other, normal, penetration, out selfContact, out otherContact);

    public readonly void GetContacts (in Polygon other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact) => Physics2D.GetContacts (this, other, normal, penetration, out selfContact, out otherContact);
}
