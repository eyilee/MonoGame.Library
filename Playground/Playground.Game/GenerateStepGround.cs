using Eycraft.Framework;
using Eycraft.Framework.World;
using System;

namespace Playground.Game;

public class GenerateStepGround : GenerateStep<GameTile>
{
    public override void Run (TileWorld<GameTile> tileWorld)
    {
        PerlinNoise perlinNoise = new (Environment.TickCount);
        float frequency = 0.1f;

        for (int x = 0; x < tileWorld.Width; x++)
        {
            for (int y = 0; y < tileWorld.Height; y++)
            {
                tileWorld.Tiles[y * tileWorld.Width + x].Value = (int)float.Round (perlinNoise.Noise (x * frequency, y * frequency));
            }
        }
    }
}
