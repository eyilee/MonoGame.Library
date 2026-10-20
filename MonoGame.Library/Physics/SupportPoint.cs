using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

internal struct SupportPoint (Vector2 self, Vector2 other)
{
    public Vector2 Self { get; set; } = self;

    public Vector2 Other { get; set; } = other;

    public readonly Vector2 Value => Self - Other;
}
