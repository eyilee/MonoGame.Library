namespace Eycraft.Framework.Graphics;

public class SpriteAnimationFrame (TextureRegion region, int duration)
{
    public TextureRegion Region { get; } = region;

    public int Duration { get; } = duration;
}
