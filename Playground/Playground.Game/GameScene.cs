using System.Collections.Generic;
using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Graphics.Shapes;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Playground;

public class GameScene : Scene
{
    private readonly SdfCircle _center = new ();

    private readonly Player _player = new ();

    private readonly Ball _ball = new ();

    private readonly Cube _cube = new ();

    private readonly List<Polygon> _polygons = [];

    private readonly PhysicsWorld _physicsWorld = new ();

    public override void Initialize ()
    {
        Vector2 position = new (Core.ScreenWidth / 2f, Core.ScreenHeight / 2f);

        _center.Position = position;
        _center.Thickness = 1f;
        _center.Color = Color.Green;
        _center.Radius = 5f;

        _player.Initialize (position, 0f);
        _player.AttachPhysics (_physicsWorld);

        //_ball.Initialize (position, 0f);
        //_ball.AttachPhysics (_physicsWorld);

        //_cube.Initialize (new Vector2 (position.X + 30f, position.Y + 30f), 1f);
        //_cube.AttachPhysics (_physicsWorld);

        for (int i = 0; i < 2; i++)
        {
            Polygon polygon = new ();

            int vertexCount = 4 + 2 + i;
            List<Vector2> vertices = new (vertexCount);

            for (int j = 0; j < vertexCount; j++)
            {
                vertices.Add (Vector2.Rotate (Vector2.UnitX * 30f, 2f * float.Pi * j / vertexCount));
            }

            polygon.Initialize (new Vector2 (position.X + (i + 1) * 60f + 100f, position.Y), 0f, vertices);
            polygon.AttachPhysics (_physicsWorld);
            _polygons.Add (polygon);
        }

        base.Initialize ();
    }

    public override void LoadContent ()
    {
        SpriteSheet sp = Content.Load<SpriteSheet> ("teemo");

        base.LoadContent ();
    }

    public override void Update (GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _player.Update (Input, deltaTime);

        //MouseState state = Mouse.GetState ();
        //_cubes[0].Position = Camera.Main.Position + state.Position.ToVector2 ();
        //_polygons[0].Position = Camera.Main.Position + state.Position.ToVector2 ();

        _physicsWorld.Update (deltaTime);

        base.Update (gameTime);
    }

    public override void Draw (GameTime gameTime)
    {
        GraphicsDevice.Clear (Color.CornflowerBlue);

        _center.Draw (Render);
        _player.Draw (Render);
        //_ball.Draw (Render);
        //_cube.Draw (Render);

        foreach (Polygon polygon in _polygons)
        {
            polygon.Draw (Render);
        }

        base.Draw (gameTime);
    }
}
