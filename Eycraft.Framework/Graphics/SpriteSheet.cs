using System.Collections.Generic;

namespace Eycraft.Framework.Graphics;

public class SpriteSheet
{
    public string Name { get; internal set; } = string.Empty;

    public Texture2DResource Texture { get; internal set; } = null!;

    public Dictionary<string, TextureRegion> Regions { get; internal set; } = [];

    public Dictionary<string, SpriteAnimation> Animations { get; internal set; } = [];

    public TextureRegion? GetRegion (string name)
    {
        if (!Regions.TryGetValue (name, out TextureRegion? region))
        {
            return null;
        }

        return region;
    }

    public SpriteAnimation? GetAnimation (string name)
    {
        if (!Animations.TryGetValue (name, out SpriteAnimation? animation))
        {
            return null;
        }

        return animation;
    }
}
