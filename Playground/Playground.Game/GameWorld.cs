using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Tilemap;
using Eycraft.Framework.World;
using System.Collections.Generic;

namespace Playground.Game;

public class GameWorld : TileWorld<GameTile>
{
    public Tilemap GroundTilemap { get; }

    public Tileset GroundTileset { get; }

    public SpriteSheet ObjectSpriteSheet { get; }

    private HashSet<Entity> Entities { get; } = [];

    public GameWorld () : base (32, 32)
    {
        GroundTilemap = new Tilemap (16, 16, 32, 32);
        GroundTileset = Core.Content.Load<Tileset> ("bitmask_reference");
        ObjectSpriteSheet = Core.Content.Load<SpriteSheet> ("milk_grass");
    }

    public void Initialize ()
    {
        Generate (new GenerateStepGround (this));
        Generate (new GenerateStepObject (this));
    }

    public void Generate (GenerateStep<GameWorld, GameTile> step)
    {
        step.Run ();
    }

    public void Draw (RenderManager render)
    {
        GroundTilemap.Draw (render);

        foreach (Entity entity in Entities)
        {
            entity.Draw (render);
        }
    }

    public void AddEntity (Entity entity)
    {
        Entities.Add (entity);
    }
}
