using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;

namespace Playground.Game.Shapes;

public class RectangleEntity : Entity
{
    private readonly SdfRectangle _shape;

    private readonly PhysicsBody _physicsBody;

    private readonly BoxCollider _collider;

    public RectangleEntity ()
    {
        _shape = AddComponent<SdfRectangle> ();
        _shape.Color = Color.White;
        _shape.Size = new Vector2 (30f, 30f);
        _shape.Filled = true;

        _physicsBody = AddComponent<PhysicsBody> ();
        _collider = _physicsBody.AddCollider<BoxCollider> ();
        _collider.Size = new Vector2 (30f, 30f);

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
