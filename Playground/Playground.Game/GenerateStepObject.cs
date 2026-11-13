using Eycraft.Framework.Graphics;
using Eycraft.Framework.World;
using System;

namespace Playground.Game;

public class GenerateStepObject (GameWorld gameWorld) : GenerateStep<GameWorld, GameTile> (gameWorld)
{
    public override void Run ()
    {
        TextureRegion? grassRegion = TileWorld.ObjectSpriteSheet.GetRegion ("grass");

        if (grassRegion == null)
        {
            throw new Exception ("Grass region not found in sprite sheet.");
        }

        int width = TileWorld.Width;
        int height = TileWorld.Height;

        Random random = new (Environment.TickCount);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameTile? tile = TileWorld.GetTile (x, y);

                if (tile == null || tile.Value == 0)
                {
                    continue;
                }

                if (random.NextDouble () < 0.05)
                {
                    Grass grass = new (grassRegion);
                    grass.Transform.Position = TileWorld.GroundTilemap.GetCenterPosition (x, y);
                    TileWorld.AddEntity (grass);
                }
            }
        }
    }
}
