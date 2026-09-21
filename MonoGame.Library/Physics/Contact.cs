using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

public struct Contact
{
    public Vector2 Normal { get; set; }

    public float Penetration { get; set; }
}
