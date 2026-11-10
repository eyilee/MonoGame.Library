using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Playground.Game.Shapes;

public class PolygonEntity : Entity
{
    private readonly PolygonShape _shape;

    private readonly PhysicsBody _physicsBody;

    private readonly PolygonCollider _collider;

    public PolygonEntity ()
    {
        _shape = AddComponent<PolygonShape> ();
        _shape.Color = Color.White;

        _physicsBody = AddComponent<PhysicsBody> ();
        _collider = _physicsBody.AddCollider<PolygonCollider> ();

        _physicsBody.OnCollisionEnter += OnCollisionEnter;
        _physicsBody.OnCollisionStay += OnCollisionStay;
        _physicsBody.OnCollisionExit += OnCollisionExit;
    }

    public void SetVertices (List<Vector2> vertices)
    {
        _shape.SetVertices (vertices);
        _collider.SetVertices (vertices);
    }

    public void AttachPhysics (PhysicsWorld physicsWorld)
    {
        physicsWorld.Add (_physicsBody);
    }

    public void DetachPhysics (PhysicsWorld physicsWorld)
    {
        physicsWorld.Remove (_physicsBody);
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

    public override void Draw (RenderManager render)
    {
        _shape.Draw (render);
    }
}
