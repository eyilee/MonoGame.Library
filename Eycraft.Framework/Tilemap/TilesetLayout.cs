namespace Eycraft.Framework.Tilemap;

public struct TilesetLayout
{
    public uint BitMask;

    public int X;

    public int Y;

    public static readonly TilesetLayout[] Blob =
    [
        new TilesetLayout () { BitMask = 0, X = 0, Y = 0 },
        new TilesetLayout () { BitMask = 4, X = 1, Y = 0 },
        new TilesetLayout () { BitMask = 84, X = 2, Y = 0 },
        new TilesetLayout () { BitMask = 92, X = 3, Y = 0 },
        new TilesetLayout () { BitMask = 124, X = 4, Y = 0 },
        new TilesetLayout () { BitMask = 116, X = 5, Y = 0 },
        new TilesetLayout () { BitMask = 80, X = 6, Y = 0 },

        new TilesetLayout () { BitMask = 16, X = 0, Y = 1 },
        new TilesetLayout () { BitMask = 28, X = 1, Y = 1 },
        new TilesetLayout () { BitMask = 117, X = 2, Y = 1 },
        new TilesetLayout () { BitMask = 95, X = 3, Y = 1 },
        new TilesetLayout () { BitMask = 255, X = 4, Y = 1 },
        new TilesetLayout () { BitMask = 253, X = 5, Y = 1 },
        new TilesetLayout () { BitMask = 113, X = 6, Y = 1 },

        new TilesetLayout () { BitMask = 21, X = 0, Y = 2 },
        new TilesetLayout () { BitMask = 87, X = 1, Y = 2 },
        new TilesetLayout () { BitMask = 221, X = 2, Y = 2 },
        new TilesetLayout () { BitMask = 127, X = 3, Y = 2 },
        //new TilesetLayout () { BitMask = 255, X = 4, Y = 2 },
        new TilesetLayout () { BitMask = 247, X = 5, Y = 2 },
        new TilesetLayout () { BitMask = 209, X = 6, Y = 2 },

        new TilesetLayout () { BitMask = 29, X = 0, Y = 3 },
        new TilesetLayout () { BitMask = 125, X = 1, Y = 3 },
        new TilesetLayout () { BitMask = 119, X = 2, Y = 3 },
        new TilesetLayout () { BitMask = 199, X = 3, Y = 3 },
        new TilesetLayout () { BitMask = 215, X = 4, Y = 3 },
        new TilesetLayout () { BitMask = 213, X = 5, Y = 3 },
        new TilesetLayout () { BitMask = 81, X = 6, Y = 3 },

        new TilesetLayout () { BitMask = 31, X = 0, Y = 4 },
        //new TilesetLayout () { BitMask = 255, X = 1, Y = 4 },
        new TilesetLayout () { BitMask = 241, X = 2, Y = 4 },
        new TilesetLayout () { BitMask = 20, X = 3, Y = 4 },
        new TilesetLayout () { BitMask = 65, X = 4, Y = 4 },
        new TilesetLayout () { BitMask = 17, X = 5, Y = 4 },
        new TilesetLayout () { BitMask = 1, X = 6, Y = 4 },

        new TilesetLayout () { BitMask = 23, X = 0, Y = 5 },
        new TilesetLayout () { BitMask = 223, X = 1, Y = 5 },
        new TilesetLayout () { BitMask = 245, X = 2, Y = 5 },
        new TilesetLayout () { BitMask = 85, X = 3, Y = 5 },
        new TilesetLayout () { BitMask = 68, X = 4, Y = 5 },
        new TilesetLayout () { BitMask = 93, X = 5, Y = 5 },
        new TilesetLayout () { BitMask = 112, X = 6, Y = 5 },

        new TilesetLayout () { BitMask = 5, X = 0, Y = 6 },
        new TilesetLayout () { BitMask = 71, X = 1, Y = 6 },
        new TilesetLayout () { BitMask = 197, X = 2, Y = 6 },
        new TilesetLayout () { BitMask = 69, X = 3, Y = 6 },
        new TilesetLayout () { BitMask = 64, X = 4, Y = 6 },
        new TilesetLayout () { BitMask = 7, X = 5, Y = 6 },
        new TilesetLayout () { BitMask = 193, X = 6, Y = 6 },
    ];
}
