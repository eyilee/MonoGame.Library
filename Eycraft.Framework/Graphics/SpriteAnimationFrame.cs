using System;

namespace Eycraft.Framework.Graphics;

public class SpriteAnimationFrame (TextureRegion region, int duration)
{
    public TextureRegion Region { get; } = region;

    public TimeSpan Duration { get; } = new TimeSpan (0, 0, 0, 0, duration);
}
