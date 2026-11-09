using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Tilemap;
using Eycraft.Framework.World;

namespace Playground.Game;

public class GameWorld
{
    private readonly TileWorld<GameTile> _tileWorld;

    private readonly Tilemap _tilemap;

    private readonly Tileset _tileset;

    public GameWorld (Tileset tileset)
    {
        _tileWorld = new TileWorld<GameTile> (32, 32);
        new GenerateStepGround ().Run (_tileWorld);

        _tilemap = new Tilemap (16, 16, 32, 32);
        _tileset = tileset;

        for (int x = 0; x < _tileWorld.Width; x++)
        {
            for (int y = 0; y < _tileWorld.Height; y++)
            {
                uint mask = GetBlobTilesetMask (x, y);

                TileEntry? tileEntry = _tileset.GetTileEntry (mask);
                if (tileEntry != null)
                {
                    _tilemap.SetTile (x, y, tileEntry.Value.Tile);
                }
            }
        }
    }

    public uint GetBlobTilesetMask (int x, int y)
    {
        uint mask = 0;

        GameTile? tile = _tileWorld.GetTile (x, y);
        if (tile == null || tile.Value != 0)
        {
            return mask;
        }

        for (int i = 0; i < Coord.Neighbors8.Length; i++)
        {
            Coord neighborCoord = Coord.Neighbors8[i];

            tile = _tileWorld.GetTile (x + neighborCoord.X, y + neighborCoord.Y);
            if (tile == null || tile.Value == 0)
            {
                mask |= 1u << i;
                continue;
            }
        }

        return mask;
    }

    public void Draw (RenderManager render)
    {
        _tilemap.Draw (render);
    }
}
