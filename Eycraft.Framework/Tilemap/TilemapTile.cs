using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Tilemap;

public class TilemapTile (int x, int y)
{
    public int X { get; } = x;

    public int Y { get; } = y;

    private readonly TileSprite _tileSprite = new ();

    private Tile? _tile;

    public Tile? Tile => _tile;

    public void SetSize (float width, float height)
    {
        _tileSprite.Position = new Vector2 (X * width, Y * height);
        _tileSprite.Size = new Vector2 (width, height);
    }

    public void SetOriginAlignment (OriginAlignment originAlignment)
    {
        _tileSprite.OriginAlignment = originAlignment;
    }

    public void SetTile (Tile? tile)
    {
        _tile = tile;
        _tileSprite.Region = tile?.Region;
    }

    public void Draw (RenderManager render)
    {
        _tileSprite.Draw (render);
    }
}
