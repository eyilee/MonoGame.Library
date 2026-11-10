using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;

namespace Playground.Game.Shapes;

public class CircleEntity : Entity
{
    private readonly SdfCircle _shape;

    private readonly PhysicsBody _physicsBody;

    private readonly CircleCollider _collider;

    public CircleEntity ()
    {
        _shape = AddComponent<SdfCircle> ();
        _shape.Color = Color.White;
        _shape.Radius = 10f;
        _shape.Filled = true;

        _physicsBody = AddComponent<PhysicsBody> ();
        _collider = _physicsBody.AddCollider<CircleCollider> ();
        _collider.Radius = 10f;

        _physicsBody.OnCollisionEnter += OnCollisionEnter;
        _physicsBody.OnCollisionStay += OnCollisionStay;
        _physicsBody.OnCollisionExit += OnCollisionExit;
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
