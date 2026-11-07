namespace Eycraft.Framework.Tilemap;

public readonly struct TileEntry (Tile tile, uint requiredMask)
{
    public Tile Tile { get; } = tile;

    public uint RequiredMask { get; } = requiredMask;
}
