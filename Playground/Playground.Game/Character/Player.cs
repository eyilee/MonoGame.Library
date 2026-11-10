using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Input;
using Eycraft.Framework.Objects;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Playground.Game.Character;

public class Player : Entity
{
    private PhysicsBody _physicsBody = null!;

    private CircleCollider _collider = null!;

    private readonly SdfCircle _shape;

    private readonly Aiming _aiming;

    private readonly ViewPoint _viewPoint = new ();

    private readonly float _maxVelocity = 300f;

    private readonly float _acceleration = 300f;

    private readonly float _deceleration = 200f;

    private readonly float _maxAngularVelocity = float.Pi;

    private readonly float _angularAcceleration = float.Pi * 4f;

    private Vector2 _velocity = Vector2.Zero;

    private float _angularVelocity = 0f;

    private bool _initialized = false;

    public Player ()
    {
        _shape = AddComponent<SdfCircle> ();
        _shape.Color = Color.Blue;
        _shape.Radius = 5f;
        _shape.Filled = true;

        _aiming = AddComponent<Aiming> ();
    }

    public void Initialize (Vector2 position, float rotation)
    {
        if (_initialized)
        {
            return;
        }

        Transform.Position = position;
        Transform.Rotation = rotation;

        _physicsBody = AddComponent<PhysicsBody> ();
        _collider = _physicsBody.AddCollider<CircleCollider> ();
        _collider.Radius = 5f;

        _viewPoint.Initialize (_aiming.Target);

        _initialized = true;
    }

    public void AttachPhysics (PhysicsWorld physicsWorld)
    {
        physicsWorld.Add (_physicsBody);
    }

    public void DetachPhysics (PhysicsWorld physicsWorld)
    {
        physicsWorld.Remove (_physicsBody);
    }

    public void Update (InputManager input, float deltaTime)
    {
        ProcessPosition (input, deltaTime);
        ProcessRotation (input, deltaTime);

        _viewPoint.Update (_aiming.Target, deltaTime);
    }

    private void ProcessPosition (InputManager input, float deltaTime)
    {
        float acceleration = 0f;

        if (input.Keyboard.IsKeyDown (Keys.A))
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

    private void ProcessRotation (InputManager input, float deltaTime)
    {
        float x = 0f;
        float y = 0f;

        if (input.Keyboard.IsKeyDown (Keys.Up))
        {
            y -= 1f;
        }

        if (input.Keyboard.IsKeyDown (Keys.Down))
        {
            y += 1f;
        }

        if (input.Keyboard.IsKeyDown (Keys.Left))
        {
            x -= 1f;
        }

        if (input.Keyboard.IsKeyDown (Keys.Right))
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

    public override void Draw (RenderManager render)
    {
        _shape.Draw (render);
        _aiming.Draw (render);
        _viewPoint.Draw (render);
    }
}
