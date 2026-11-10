using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Tilemap;

public class TilemapTile : Entity
{
    public int X { get; }

    public int Y { get; }

    private readonly Sprite _sprite;

    private Tile? _tile;

    public Tile? Tile => _tile;

    public TilemapTile (int x, int y)
    {
        X = x;
        Y = y;
        _sprite = AddComponent<Sprite> ();
    }

    public void SetSize (int width, int height)
    {
        Transform.Position = new Vector2 (X * width, Y * height);
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

    public override void Draw (RenderManager render)
    {
        if (_tile != null)
        {
            _sprite.Draw (render);
        }
    }
}
