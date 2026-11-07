using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Tilemap;

public class TilemapTile
{
    private readonly int _x;

    public int X => _x;

    private readonly int _y;

    public int Y => _y;

    private readonly Sprite _sprite = new ();

    private Tile? _tile;

    public Tile? Tile => _tile;

    public TilemapTile (int x, int y, int width, int height)
    {
        _x = x;
        _y = y;
        SetSize (width, height);
    }

    public void SetSize (int width, int height)
    {
        _sprite.Position = new Vector2 (_x * width, _y * height);
        _sprite.PreferredSize = new Vector2 (width, height);
    }

    public void SetTile (Tile tile)
    {
        _tile = tile;
        _sprite.Region = tile.Region;
    }

    public void RemoveTile ()
    {
        _tile = null;
        _sprite.Region = null;
    }

    public void Draw (RenderManager render)
    {
        if (_tile != null)
        {
            _sprite.Draw (render);
        }
    }
}
