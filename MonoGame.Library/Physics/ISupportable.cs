using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

public interface ISupportable
{
    public Vector2 Center { get; }

    public Vector2 Support (Vector2 direction);
}
