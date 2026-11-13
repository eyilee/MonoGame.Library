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

    private Vector2 _origin = Vector2.Zero;

    private OriginAlignment _originAlignment = OriginAlignment.TopLeft;

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

        AlignOrigin ();
    }

    private void AlignOrigin ()
    {
        float x = _tileWidth;
        float y = _tileHeight;

        _origin = _originAlignment switch
        {
            OriginAlignment.None => _origin,
            OriginAlignment.TopLeft => new Vector2 (0f, 0f),
            OriginAlignment.TopCenter => new Vector2 (x * 0.5f, 0f),
            OriginAlignment.TopRight => new Vector2 (x, 0f),
            OriginAlignment.CenterLeft => new Vector2 (0f, y * 0.5f),
            OriginAlignment.Center => new Vector2 (x * 0.5f, y * 0.5f),
            OriginAlignment.CenterRight => new Vector2 (x, y * 0.5f),
            OriginAlignment.BottomLeft => new Vector2 (0f, y),
            OriginAlignment.BottomCenter => new Vector2 (x * 0.5f, y),
            OriginAlignment.BottomRight => new Vector2 (x, y),
            _ => _origin
        };
    }

    public void SetOriginAlignment (OriginAlignment originAlignment)
    {
        _originAlignment = originAlignment;

        foreach ((_, TilemapChunk chunk) in _chunks)
        {
            chunk.SetOriginAlignment (originAlignment);
        }

        AlignOrigin ();
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

    public Vector2 GetPosition (int x, int y) => new (x * _tileWidth, y * _tileHeight);

    public Vector2 GetCenterPosition (int x, int y) => new (x * _tileWidth - _origin.X + _tileWidth * 0.5f, y * _tileHeight - _origin.Y + _tileHeight * 0.5f);

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
