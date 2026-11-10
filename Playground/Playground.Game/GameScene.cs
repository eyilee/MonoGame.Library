using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Playground.Game.Character;
using Playground.Game.Shapes;
using System.Collections.Generic;

namespace Playground.Game;

public class GameScene : Scene
{
    private readonly Origin _origin = new ();

    private readonly CircleEntity _circle = new ();

    private readonly LineEntity _line = new ();

    private readonly ParabolaEntity _parabola = new ();

    private readonly RectangleEntity _rectangle = new ();

    private readonly PolygonEntity _polygon = new ();

    private readonly Player _player = new ();

    private readonly Cluster _cluster = new ();

    //private readonly AnimatedSprite _teemo = new ();

    private GameWorld? _gameWorld;

    private readonly PhysicsWorld _physicsWorld = new ();

    public override void Initialize ()
    {
        Vector2 position = new (Core.ScreenWidth / 2f, Core.ScreenHeight / 2f);

        _origin.Transform.Position = position;

        _circle.Transform.Position = position + Vector2.UnitX * 50f;
        _circle.AttachPhysics (_physicsWorld);

        _line.Transform.Position = position + Vector2.UnitX * 50f + Vector2.UnitY * 50f;

        _parabola.Transform.Position = position + Vector2.UnitX * 100f;

        _rectangle.Transform.Position = position + Vector2.UnitX * 100f + Vector2.UnitY * 50f;
        _rectangle.AttachPhysics (_physicsWorld);

        int vertexCount = 7;
        List<Vector2> vertices = new (vertexCount);

        for (int i = 0; i < vertexCount; i++)
        {
            vertices.Add (Vector2.Rotate (Vector2.UnitX * 25f, float.Tau * i / vertexCount));
        }

        _polygon.Transform.Position = position + Vector2.UnitX * 150f;
        _polygon.SetVertices (vertices);
        _polygon.AttachPhysics (_physicsWorld);

        _player.Initialize (position, 0f);
        _player.AttachPhysics (_physicsWorld);

        base.Initialize ();
    }

    public override void LoadContent ()
    {
        SpriteSheet sp = Content.Load<SpriteSheet> ("teemo");

        //_teemo.SpriteSheet = sp;
        //_teemo.Position = new Vector2 (Core.ScreenWidth / 2f, Core.ScreenHeight / 2f);
        //_teemo.PreferredSize = new Vector2 (100f, 100f);
        //_teemo.OriginAlignment = OriginAlignment.BottomCenter;
        //_teemo.PlayAnimation ("teemo_up");

        _gameWorld = new GameWorld ();
        _gameWorld.Initialize ();

        base.LoadContent ();
    }

    public override void Update (GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _player.Update (Input, deltaTime);
        //_teemo.Update (gameTime);

        //MouseState state = Mouse.GetState ();
        //_cubes[0].Position = Camera.Main.Position + state.Position.ToVector2 ();
        //_polygons[0].Position = Camera.Main.Position + state.Position.ToVector2 ();

        _physicsWorld.Update (deltaTime);

        base.Update (gameTime);
    }

    public override void Draw (GameTime gameTime)
    {
        GraphicsDevice.Clear (Color.CornflowerBlue);

        _origin.Draw (Render);
        _circle.Draw (Render);
        _line.Draw (Render);
        _parabola.Draw (Render);
        _rectangle.Draw (Render);
        _polygon.Draw (Render);

        //_gameWorld?.Draw (Render);
        _player.Draw (Render);
        //_ball.Draw (Render);
        //_cube.Draw (Render);
        //_teemo.Draw (Render);

        base.Draw (gameTime);
    }
}
