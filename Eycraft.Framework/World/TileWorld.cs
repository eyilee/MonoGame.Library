namespace Eycraft.Framework.World;

public class TileWorld<TTileData> where TTileData : TileData, new()
{
    public int Width { get; private set; }

    public int Height { get; private set; }

    public TTileData[] Tiles { get; private set; } = [];

    public void Initialize (int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new TTileData[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tiles[y * width + x] = new TTileData ()
                {
                    X = x,
                    Y = y,
                    Coord = new Coord (x, y)
                };
            }
        }

        for (int i = 0; i < Tiles.Length; i++)
        {
            Tiles[i] = new TTileData ();
        }
    }

    public TTileData? GetTile (int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return null;
        }

        return Tiles[y * Width + x];
    }

    public TTileData? GetTile (Coord coord) => GetTile (coord.X, coord.Y);
}
