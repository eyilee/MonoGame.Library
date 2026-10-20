using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Collections.Generic;

namespace MonoGame.Library.Content.Pipeline;

public sealed class SpriteSheetDocument
{
    public string Sprite { get; set; } = string.Empty;

    public Dictionary<string, SpriteSheetRegionDocument> Regions { get; set; } = [];

    public Dictionary<string, SpriteSheetAnimationDocument> Animations { get; set; } = [];

    public TextureContent? Texture { get; set; } = null;
}

public sealed class SpriteSheetRegionDocument
{
    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}

public sealed class SpriteSheetAnimationDocument
{
    public List<SpriteSheetAnimationFrameDocument> Frames { get; set; } = [];
}

public sealed class SpriteSheetAnimationFrameDocument
{
    public string Region { get; set; } = string.Empty;

    public int Duration { get; set; }
}
