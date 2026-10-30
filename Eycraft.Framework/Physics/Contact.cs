using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Physics;

public struct Contact
{
    public Vector2 Point { get; set; }

    public Vector2 Normal { get; set; }

    public float Penetration { get; set; }
}
