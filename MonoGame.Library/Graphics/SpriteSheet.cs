using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Library.Graphics;

public class SpriteSheet
{
    public string Name { get; set; } = string.Empty;

    public Dictionary<string, SpriteSheetRegion> Regions { get; set; } = [];

    public Dictionary<string, SpriteSheetAnimation> Animations { get; set; } = [];

    public Texture2D Texture { get; set; } = null!;
}

public sealed class SpriteSheetRegion
{
    public string Name { get; set; } = string.Empty;

    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}

public sealed class SpriteSheetAnimation
{
    public string Name { get; set; } = string.Empty;

    public List<SpriteSheetAnimationFrame> Frames { get; set; } = [];
}

public sealed class SpriteSheetAnimationFrame
{
    public string Region { get; set; } = string.Empty;

    public int Duration { get; set; }
}
