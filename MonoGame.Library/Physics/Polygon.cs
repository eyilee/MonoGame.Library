using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public struct Polygon (List<Vector2> points) : ISupportable
{
    public List<Vector2> Points { get; set; } = points;

    public readonly Vector2 Position => Points[0];

    public readonly Vector2 Support (Vector2 direction)
    {
        float maxValue = float.MinValue;
        Vector2 farthestPoint = Vector2.Zero;

        foreach (Vector2 point in Points)
        {
            float value = Vector2.Dot (point, direction);

            if (value > maxValue)
            {
                maxValue = value;
                farthestPoint = point;
            }
        }

        return farthestPoint;
    }

    public readonly void GetContacts (in ISupportable other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact) => other.GetContacts (this, -normal, penetration, out otherContact, out selfContact);

    public readonly void GetContacts (in Circle other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact) => Physics2D.GetContacts (this, other, normal, penetration, out selfContact, out otherContact);

    public readonly void GetContacts (in Polygon other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact) => Physics2D.GetContacts (this, other, normal, penetration, out selfContact, out otherContact);
}
