using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Eycraft.Framework.Physics;
using Eycraft.Framework.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Playground.Game.Character;

public class Player : Entity
{
    private readonly AnimatedSprite _sprite;

    private readonly PhysicsBody _physicsBody;

    private readonly CircleCollider _collider;

    private readonly Aiming _aiming;

    private readonly TrackingCamera _trackingCamera;

    private readonly float _maxVelocity = 300f;

    private readonly float _acceleration = 300f;

    private readonly float _deceleration = 200f;

    private readonly float _maxAngularVelocity = float.Pi;

    private readonly float _angularAcceleration = float.Pi * 4f;

    private Vector2 _velocity = Vector2.Zero;

    private float _angularVelocity = 0f;

    private Direction _direction = new (Direction.Down);

    private bool _isMoving = false;

    public Player ()
    {
        _sprite = AddComponent<AnimatedSprite> ();
        _sprite.PreferredSize = new Vector2 (32f, 32f);
        _sprite.SpriteSheet = Core.Content.Load<SpriteSheet> ("teemo");
        _sprite.PlayAnimation ("idle_down");

        _physicsBody = AddComponent<PhysicsBody> ();

        _collider = _physicsBody.AddCollider<CircleCollider> ();
        _collider.Radius = 16f;

        _aiming = AddComponent<Aiming> ();
        _aiming.Length = 50f;

        _trackingCamera = AddComponent<TrackingCamera> ();
        _trackingCamera.Camera = Camera.Main;
    }

    public void AttachPhysics (PhysicsWorld physicsWorld)
    {
        physicsWorld.Add (_physicsBody);
    }

    public void DetachPhysics (PhysicsWorld physicsWorld)
    {
        physicsWorld.Remove (_physicsBody);
    }

    public override void Update (GameTime gameTime)
    {
        _sprite.Update (gameTime);

        ProcessPosition (gameTime);
        ProcessRotation (gameTime);

        _trackingCamera.Target = _aiming.Target;
        _trackingCamera.Update (gameTime);

        UpdateActor ();
    }

    private void ProcessPosition (GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float acceleration = 0f;

        if (Core.Input.Keyboard.IsKeyDown (Keys.A))
        {
            acceleration = _acceleration;
        }

        if (acceleration != 0f)
        {
            float deltaRotation = MathHelper.WrapAngle (Transform.Rotation - float.Atan2 (_velocity.Y, _velocity.X));

            if (float.Abs (deltaRotation) < float.Pi / 6f)
            {
                _velocity.Rotate (deltaRotation);
            }

            Vector2 direction = new (float.Cos (Transform.Rotation), float.Sin (Transform.Rotation));

            _velocity += direction * acceleration * deltaTime;

            if (_velocity.LengthSquared () > 0f)
            {
                direction = Vector2.Normalize (_velocity);
            }

            _velocity = direction * float.Min (_velocity.Length (), _maxVelocity);
        }
        else
        {
            float velocity = float.Max (_velocity.Length () - _deceleration * deltaTime, 0f);

            Vector2 direction = _velocity;

            if (direction.LengthSquared () > 0f)
            {
                direction.Normalize ();
            }

            _velocity = direction * float.Min (velocity, _maxVelocity);
        }

        Transform.Position += _velocity * deltaTime;
    }

    private void ProcessRotation (GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float x = 0f;
        float y = 0f;

        if (Core.Input.Keyboard.IsKeyDown (Keys.Up))
        {
            y -= 1f;
        }

        if (Core.Input.Keyboard.IsKeyDown (Keys.Down))
        {
            y += 1f;
        }

        if (Core.Input.Keyboard.IsKeyDown (Keys.Left))
        {
            x -= 1f;
        }

        if (Core.Input.Keyboard.IsKeyDown (Keys.Right))
        {
            x += 1f;
        }

        if (x == 0f && y == 0f)
        {
            _angularVelocity = 0f;
            return;
        }

        float targetAngle = float.Atan2 (y, x);
        float delta = MathHelper.WrapAngle (targetAngle - Transform.Rotation);

        _angularVelocity += _angularAcceleration * deltaTime * float.Sign (delta);
        _angularVelocity = float.Clamp (_angularVelocity, -_maxAngularVelocity, _maxAngularVelocity);

        float maxStep = float.Abs (_angularVelocity * deltaTime);
        float step = float.Clamp (delta, -maxStep, maxStep);

        Transform.Rotation += step;
    }

    private void UpdateActor ()
    {
        Direction direction = Direction.FromRotation (Transform.Rotation);
        bool isMoving = _velocity.LengthSquared () > 0f;

        if (_direction != direction || _isMoving != isMoving)
        {
            _direction = direction;
            _isMoving = isMoving;

            switch (direction.Value)
            {
                case Direction.Up:
                    _sprite.PlayAnimation (isMoving ? "walk_up" : "idle_up");
                    break;
                case Direction.Down:
                    _sprite.PlayAnimation (isMoving ? "walk_down" : "idle_down");
                    break;
                case Direction.Left:
                    _sprite.PlayAnimation (isMoving ? "walk_left" : "idle_left");
                    break;
                case Direction.Right:
                    _sprite.PlayAnimation (isMoving ? "walk_right" : "idle_right");
                    break;
                default:
                    break;
            }
        }
    }

    public override void Draw (RenderManager render)
    {
        _sprite.Draw (render);
        _aiming.Draw (render);
    }
}
