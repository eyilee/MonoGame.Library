using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Eycraft.Framework.Tilemap;

public class Tilemap
{
    private int _chunkWidth;

    private int _chunkHeight;

    private Dictionary<Point, TilemapChunk> _chunks = [];

    private int _tileWidth;

    private int _tileHeight;

    public Tilemap (int chunkWidth, int chunkHeight, int tileWidth, int tileHeight)
    {
        SetChunkSize (chunkWidth, chunkHeight);
        SetTileSize (tileWidth, tileHeight);
    }

    public void SetChunkSize (int chunkWidth, int chunkHeight)
    {
        if (_chunkWidth == chunkWidth && _chunkHeight == chunkHeight)
        {
            return;
        }

        _chunkWidth = chunkWidth;
        _chunkHeight = chunkHeight;

        Dictionary<Point, TilemapChunk> newChunks = [];

        foreach ((Point chunkIndex, TilemapChunk chunk) in _chunks)
        {
            foreach (TilemapTile? chunkTile in chunk.ChunkTiles)
            {
                if (chunkTile != null)
                {
                    int x = chunkTile.X;
                    int y = chunkTile.Y;
                    Point newChunkIndex = new (x / _chunkWidth, y / _chunkHeight);

                    if (!newChunks.TryGetValue (newChunkIndex, out TilemapChunk? newChunk))
                    {
                        newChunk = new TilemapChunk (newChunkIndex.X, newChunkIndex.Y, _chunkWidth, _chunkHeight);
                        newChunk.SetTileSize (_tileWidth, _tileHeight);
                        newChunks.Add (newChunkIndex, newChunk);
                    }

                    int localX = x % _chunkWidth;
                    int localY = y % _chunkHeight;
                    newChunk.SetChunkTile (localX, localY, chunkTile);
                }
            }
        }

        _chunks = newChunks;
    }

    public void SetTileSize (int tileWidth, int tileHeight)
    {
        if (_tileWidth == tileWidth && _tileHeight == tileHeight)
        {
            return;
        }

        _tileWidth = tileWidth;
        _tileHeight = tileHeight;

        foreach ((_, TilemapChunk chunk) in _chunks)
        {
            chunk.SetTileSize (tileWidth, tileHeight);
        }
    }

    public void SetTile (int x, int y, Tile tile)
    {
        Point chunkIndex = new (x / _chunkWidth, y / _chunkHeight);

        if (!_chunks.TryGetValue (chunkIndex, out TilemapChunk? chunk))
        {
            chunk = new TilemapChunk (chunkIndex.X, chunkIndex.Y, _chunkWidth, _chunkHeight);
            chunk.SetTileSize (_tileWidth, _tileHeight);
            _chunks.Add (chunkIndex, chunk);
        }

        int localX = x % _chunkWidth;
        int localY = y % _chunkHeight;
        chunk.SetTile (localX, localY, tile);
    }

    public void Draw (RenderManager render)
    {
        // TODO: Implement frustum culling to only draw visible chunks and tiles.
        foreach ((Point chunkIndex, TilemapChunk chunk) in _chunks)
        {
            foreach (TilemapTile? chunkTile in chunk.ChunkTiles)
            {
                chunkTile?.Draw (render);
            }
        }
    }
}
