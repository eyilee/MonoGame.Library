using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Library;
using MonoGame.Library.Graphics;

namespace Test;

public class GameScene : Scene
{
    private SdfCircle _actor = null!;

    private SdfCircle _aim = null!;

    private Vector2 _position = new (Core.ScreenWidth / 2f, Core.ScreenHeight / 2f);

    private Vector2 _direction = new (50f, 0f);

    private float _rotation = 0f;

    private float _rotationSpeed = float.Pi / 2f;

    // right: 0
    // down: 1/4
    // left: 1/2
    // up: 3/4

    public override void Initialize ()
    {
        base.Initialize ();
    }

    public override void LoadContent ()
    {
        Vector2 offset = _direction;
        offset.Rotate (_rotation);

        Vector2 aim = _position + offset;

        _actor = new SdfCircle
        {
            Position = _position,
            Thickness = 3f,
            Color = Color.Blue,
            Radius = 5f
        };

        _aim = new SdfCircle
        {
            Position = aim,
            Thickness = 1f,
            Color = Color.Red,
            Radius = 3f
        };

        base.LoadContent ();
    }

    public override void UnloadContent ()
    {
        base.UnloadContent ();
    }

    protected override void Dispose (bool disposing)
    {
        if (disposing)
        {
        }

        base.Dispose (disposing);
    }

    public override void Update (GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        float x = 0f;
        float y = 0f;

        if (Input.Keyboard.IsKeyDown (Keys.Up))
        {
            y -= 1f;
        }

        if (Input.Keyboard.IsKeyDown (Keys.Down))
        {
            y += 1f;
        }

        if (Input.Keyboard.IsKeyDown (Keys.Left))
        {
            x -= 1f;
        }

        if (Input.Keyboard.IsKeyDown (Keys.Right))
        {
            x += 1f;
        }

        if (x != 0f || y != 0f)
        {
            float r = float.Atan2 (y, x);
            _rotation += ToAngle (r, deltaTime);
            _rotation %= float.Pi * 2f;
        }

        Vector2 offset = _direction;
        offset.Rotate (_rotation);

        Vector2 aim = _position + offset;
        _aim.Position = aim;

        base.Update (gameTime);
    }

    private float ToAngle (float angle, float deltaTime)
    {
        float d = angle - _rotation;

        if (float.Abs (d) >= float.Pi)
        {
            if (d > 0)
            {
                d -= float.Pi * 2f;
            }
            else if (d < 0)
            {
                d += float.Pi * 2f;
            }
        }

        float amount = 0;

        if (d > 0)
        {
            amount = float.Min (d, _rotationSpeed * deltaTime);
        }
        else if (d < 0)
        {
            amount = float.Max (d, -_rotationSpeed * deltaTime);
        }

        return amount;
    }

    public override void Draw (GameTime gameTime)
    {
        GraphicsDevice.Clear (Color.CornflowerBlue);

        _actor.Draw (Render);
        _aim.Draw (Render);

        base.Draw (gameTime);
    }
}
