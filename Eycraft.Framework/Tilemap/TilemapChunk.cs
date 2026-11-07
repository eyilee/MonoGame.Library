using System;

namespace Eycraft.Framework.Tilemap;

public class TilemapChunk
{
    private readonly int _x;

    public int X => _x;

    private readonly int _y;

    public int Y => _y;

    private readonly int _width;

    private readonly int _height;

    private readonly TilemapTile?[] _chunkTiles;

    public TilemapTile?[] ChunkTiles => _chunkTiles;

    private int _tileWidth;

    private int _tileHeight;

    public TilemapChunk (int x, int y, int width, int height, int tileWidth, int tileHeight)
    {
        _x = x;
        _y = y;
        _width = width;
        _height = height;
        _chunkTiles = new TilemapTile?[_width * _height];
        SetSize (tileWidth, tileHeight);
    }

    public void SetSize (int tileWidth, int tileHeight)
    {
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;

        foreach (TilemapTile? chunkTile in _chunkTiles)
        {
            chunkTile?.SetSize (tileWidth, tileHeight);
        }
    }

    public TilemapTile? GetChunkTile (int x, int y)
    {
        return _chunkTiles[y * _width + x];
    }

    public Span<TilemapTile?> GetChunkTiles (int x, int y, int length)
    {
        return new Span<TilemapTile?> (_chunkTiles, y * _width + x, length);
    }

    public void SetChunkTile (int x, int y, TilemapTile chunkTile)
    {
        _chunkTiles[y * _width + x] = chunkTile;
    }

    public void RemoveChunkTile (int x, int y)
    {
        _chunkTiles[y * _width + x] = null;
    }

    public Tile? GetTile (int x, int y)
    {
        return _chunkTiles[y * _width + x]?.Tile;
    }

    public void SetTile (int x, int y, Tile tile)
    {
        int index = y * _width + x;
        TilemapTile? chunkTile = _chunkTiles[index];

        if (chunkTile == null)
        {
            chunkTile = new TilemapTile (_x * _width + x, _y * _height + y, _tileWidth, _tileHeight);
            _chunkTiles[index] = chunkTile;
        }

        chunkTile.SetTile (tile);
    }

    public void RemoveTile (int x, int y)
    {
        _chunkTiles[y * _width + x]?.RemoveTile ();
    }
}
