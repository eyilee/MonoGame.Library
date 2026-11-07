using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Playground.Game;

public class Polygon : Entity
{
    private PhysicsBody _physicsBody = null!;

    private PolygonCollider _collider = null!;

    private readonly PolygonShape _shape = new ();

    private bool _initialized = false;

    public void Initialize (Vector2 position, float rotation, List<Vector2> vertices)
    {
        if (_initialized)
        {
            return;
        }

        Transform.OnTransformChanged += OnTransformChanged;
        Transform.Position = position;
        Transform.Rotation = rotation;

        _physicsBody = AddComponent<PhysicsBody> ();
        _collider = _physicsBody.AddCollider<PolygonCollider> ();
        _collider.SetVertices (vertices);

        _shape.Color = Color.White;
        _shape.Position = position;
        _shape.SetVertices (vertices);

        _physicsBody.OnCollisionEnter += OnCollisionEnter;
        _physicsBody.OnCollisionStay += OnCollisionStay;
        _physicsBody.OnCollisionExit += OnCollisionExit;

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

    private void OnTransformChanged (Transform.ChangedFlags changedFlags)
    {
        _shape.Position = Transform.Position;
    }

    private void OnCollisionEnter (PhysicsBodyCollision collision)
    {
        _shape.Color = Color.Red;
    }

    private void OnCollisionStay (PhysicsBodyCollision collision)
    {
        _shape.Color = Color.Red;
    }

    private void OnCollisionExit (PhysicsBodyCollision collision)
    {
        _shape.Color = Color.White;
    }

    public void Draw (RenderManager render)
    {
        if (!_initialized)
        {
            return;
        }

        _shape.Draw (render);
    }
}
