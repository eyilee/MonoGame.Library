using System;

namespace Eycraft.Framework.Tilemap;

public class TilemapChunk (int x, int y, int width, int height)
{
    public int X { get; } = x;

    public int Y { get; } = y;

    public int Width { get; } = width;

    public int Height { get; } = height;

    public TilemapTile?[] ChunkTiles { get; } = new TilemapTile?[width * height];

    private int _tileWidth;

    private int _tileHeight;

    public void SetTileSize (int tileWidth, int tileHeight)
    {
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;

        foreach (TilemapTile? chunkTile in ChunkTiles)
        {
            chunkTile?.SetSize (tileWidth, tileHeight);
        }
    }

    public TilemapTile? GetChunkTile (int x, int y)
    {
        return ChunkTiles[y * Width + x];
    }

    public Span<TilemapTile?> GetChunkTiles (int x, int y, int length)
    {
        return new Span<TilemapTile?> (ChunkTiles, y * Width + x, length);
    }

    public void SetChunkTile (int x, int y, TilemapTile? chunkTile)
    {
        ChunkTiles[y * Width + x] = chunkTile;
    }

    public Tile? GetTile (int x, int y)
    {
        return ChunkTiles[y * Width + x]?.Tile;
    }

    public void SetTile (int x, int y, Tile? tile)
    {
        int index = y * Width + x;
        TilemapTile? chunkTile = ChunkTiles[index];

        if (chunkTile == null)
        {
            chunkTile = new TilemapTile (X * Width + x, Y * Height + y);
            chunkTile.SetSize (_tileWidth, _tileHeight);
            ChunkTiles[index] = chunkTile;
        }

        chunkTile.SetTile (tile);
    }
}
