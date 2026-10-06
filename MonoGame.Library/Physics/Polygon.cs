using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public struct Polygon (List<Vector2> points) : ISupportable
{
    public List<Vector2> Points = points;

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
}
