using Eycraft.Framework.Graphics;
using Eycraft.Framework.Tilemap;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

namespace Eycraft.Framework.Content.Readers;

public sealed class TilesetReader : ContentTypeReader<Tileset>
{
    protected override Tileset Read (ContentReader input, Tileset existingInstance)
    {
        Tileset tileset = existingInstance ?? new Tileset ();

        tileset.Name = input.ReadString ();
        tileset.Texture = new (tileset.Name, input.ReadObject<Texture2D> ());
        tileset.TileWidth = input.ReadInt32 ();
        tileset.TileHeight = input.ReadInt32 ();
        tileset.TileEntries = [];
        tileset.TileEntries.Capacity = TilesetLayout.Blob.Length;

        foreach (TilesetLayout layout in TilesetLayout.Blob)
        {
            int x = layout.X * tileset.TileWidth;
            int y = layout.Y * tileset.TileHeight;
            TextureRegion region = new (tileset.Texture, x, y, tileset.TileWidth, tileset.TileHeight);
            Tile tile = new (region);
            uint requiredMask = Tileset.NormalizeMask (layout.BitMask);
            tileset.TileEntries.Add (new TileEntry (tile, requiredMask));
        }

        tileset.TileEntries.Sort ((TileEntry lhs, TileEntry rhs) => BitOperations.PopCount (rhs.RequiredMask).CompareTo (BitOperations.PopCount (lhs.RequiredMask)));

        return tileset;
    }
}
