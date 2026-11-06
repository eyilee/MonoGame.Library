using Eycraft.Framework.Graphics;
using System.Collections.Generic;

namespace Eycraft.Framework.Tilemap;

public partial class Tileset
{
    public string Name { get; internal set; } = string.Empty;

    public Texture2DResource Texture { get; internal set; } = null!;

    public int TileWidth { get; internal set; }

    public int TileHeight { get; internal set; }

    public List<TileEntry> TileEntries { get; internal set; } = [];

    public TileEntry? GetTileEntry (uint requiredMask)
    {
        requiredMask = NormalizeMask (requiredMask);

        foreach (TileEntry entry in TileEntries)
        {
            if ((entry.RequiredMask & requiredMask) == entry.RequiredMask)
            {
                return entry;
            }
        }

        return null;
    }

    internal static uint NormalizeMask (uint mask)
    {
        if ((mask & 1u) == 0u || (mask & 4u) == 0u)
        {
            mask &= ~2u; // NE
        }

        if ((mask & 4u) == 0u || (mask & 16u) == 0u)
        {
            mask &= ~8u; // SE
        }

        if ((mask & 16u) == 0u || (mask & 64u) == 0u)
        {
            mask &= ~32u; // SW
        }

        if ((mask & 64u) == 0u || (mask & 1u) == 0u)
        {
            mask &= ~128u; // NW
        }

        return mask & 0xFFu;
    }
}
