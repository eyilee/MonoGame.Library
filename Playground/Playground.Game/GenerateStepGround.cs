using Eycraft.Framework;
using Eycraft.Framework.Tilemap;
using Eycraft.Framework.World;
using System;

namespace Playground.Game;

public class GenerateStepGround (GameWorld gameWorld) : GenerateStep<GameWorld, GameTile> (gameWorld)
{
    public override void Run ()
    {
        int width = TileWorld.Width;
        int height = TileWorld.Height;

        PerlinNoise perlinNoise = new (Environment.TickCount);
        float frequency = 0.1f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileWorld.Tiles[y * width + x].Value = (int)float.Round (perlinNoise.Noise (x * frequency, y * frequency));
            }
        }

        Tilemap tilemap = TileWorld.GroundTilemap;
        Tileset tileset = TileWorld.GroundTileset;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                uint mask = GetBlobTilesetMask (x, y);
                TileEntry? tileEntry = tileset.GetTileEntry (mask);

                if (tileEntry != null)
                {
                    tilemap.SetTile (x, y, tileEntry.Value.Tile);
                }
            }
        }
    }

    public uint GetBlobTilesetMask (int x, int y)
    {
        uint mask = 0;

        GameTile? tile = TileWorld.GetTile (x, y);

        if (tile == null || tile.Value != 0)
        {
            return mask;
        }

        for (int i = 0; i < Coord.Neighbors8.Length; i++)
        {
            Coord neighborCoord = Coord.Neighbors8[i];
            tile = TileWorld.GetTile (x + neighborCoord.X, y + neighborCoord.Y);

            if (tile == null || tile.Value == 0)
            {
                mask |= 1u << i;

                continue;
            }
        }

        return mask;
    }
}
