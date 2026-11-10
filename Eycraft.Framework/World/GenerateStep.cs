namespace Eycraft.Framework.World;

public abstract class GenerateStep<TTileWorld, TTileData> (TTileWorld tileWorld)
    where TTileWorld : TileWorld<TTileData>, new()
    where TTileData : TileData, new()
{
    public TTileWorld TileWorld { get; private set; } = tileWorld;

    public abstract void Run ();
}
