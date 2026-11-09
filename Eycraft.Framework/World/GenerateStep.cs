namespace Eycraft.Framework.World;

public abstract class GenerateStep<TTileData> where TTileData : TileData, new()
{
    public abstract void Run (TileWorld<TTileData> tileWorld);
}
