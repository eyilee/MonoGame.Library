using Eycraft.Framework.Graphics;

namespace Eycraft.Framework.Tilemap;

public readonly struct TileEntry (TextureRegion region, uint requiredMask)
{
    public TextureRegion Region { get; } = region;

    public uint RequiredMask { get; } = requiredMask;
}
