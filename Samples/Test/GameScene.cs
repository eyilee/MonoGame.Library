using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Library;
using MonoGame.Library.Graphics;

namespace Test;

public class ViewPoint
{
    private readonly SdfCircle _shape = new ();

    private Vector2 _position = Vector2.Zero;

    private Vector2 _velocity = Vector2.Zero;

    private readonly float _targetLength = 5f;

    private readonly float _damping = 0.9f;

    public ViewPoint (Vector2 position, Color color, float radius)
    {
        _shape.Position = position;
        _shape.Color = color;
        _shape.Radius = radius;
        _position = position;
    }

    public void Update (Vector2 target, float deltaTime)
    {
        float length = Vector2.Distance (_position, target);

        if (length <= _targetLength)
        {
            _velocity = Vector2.Zero;
            return;
        }

        Vector2 acceleration = (target - _position) / length * (length - _targetLength);
        _velocity += acceleration;
        _position += _velocity * deltaTime;
        _velocity *= _damping;

        _shape.Position = _position;
        Camera.Main.LookAt (_position);
    }

    public void Draw (RenderManager render)
    {
        _shape.Draw (render);
    }
}

public class GameScene : Scene
{
    private SdfCircle _center = null!;

    private SdfCircle _actor = null!;

    private SdfCircle _aim = null!;

    private ViewPoint _viewPoint = null!;

    private Vector2 _indicator = new (50f, 0f);

    private float _rotation = 0f;

    private readonly float _moveSpeed = 100f;

    private readonly float _rotationSpeed = float.Pi;

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
        Vector2 position = new (Core.ScreenWidth / 2f, Core.ScreenHeight / 2f);

        _center = new SdfCircle
        {
            Position = position,
            Thickness = 1f,
            Color = Color.Green,
            Radius = 5f
        };

        Vector2 offset = _indicator;
        offset.Rotate (_rotation);

        Vector2 aim = position + offset;

        _actor = new SdfCircle
        {
            Position = position,
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

        _viewPoint = new ViewPoint (aim, Color.Yellow, 5f);

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

        Move (deltaTime);
        Rotate (deltaTime);

        Vector2 position = _actor.Position;
        Vector2 offset = _indicator;
        offset.Rotate (_rotation);

        Vector2 aim = position + offset;
        _aim.Position = aim;
        _viewPoint.Update (aim, deltaTime);

        base.Update (gameTime);
    }

    private void Move (float deltaTime)
    {
        float x = 0f;
        float y = 0f;

        if (Input.Keyboard.IsKeyDown (Keys.W))
        {
            y -= 1f;
        }

        if (Input.Keyboard.IsKeyDown (Keys.S))
        {
            y += 1f;
        }

        if (Input.Keyboard.IsKeyDown (Keys.A))
        {
            x -= 1f;
        }

        if (Input.Keyboard.IsKeyDown (Keys.D))
        {
            x += 1f;
        }

        if (x != 0f || y != 0f)
        {
            Vector2 direction = new (x, y);
            direction.Normalize ();

            _actor.Position += direction * _moveSpeed * deltaTime;
        }
    }

    private void Rotate (float deltaTime)
    {
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

        _center.Draw (Render);
        _actor.Draw (Render);
        _aim.Draw (Render);
        _viewPoint.Draw (Render);

        base.Draw (gameTime);
    }
}
