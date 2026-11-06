using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Eycraft.Framework.Content.Pipeline;

public sealed class TilesetDocument
{
    public string Name { get; set; } = string.Empty;

    public int TileWidth { get; set; }

    public int TileHeight { get; set; }

    public TextureContent? Texture { get; set; } = null;
}
