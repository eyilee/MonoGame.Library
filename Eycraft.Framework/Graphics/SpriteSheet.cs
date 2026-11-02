using System.Collections.Generic;

namespace Eycraft.Framework.Graphics;

public class SpriteSheet
{
    public string Name { get; internal set; } = string.Empty;

    public Texture2DResource Texture { get; internal set; } = null!;

    public Dictionary<string, TextureRegion> Regions { get; internal set; } = [];

    public Dictionary<string, SpriteAnimation> Animations { get; internal set; } = [];
}
