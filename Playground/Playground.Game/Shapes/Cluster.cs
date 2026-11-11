using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Playground.Game.Shapes;

public class Cluster : Entity
{
    private readonly SdfCircle _circleShape;

    private readonly SdfRectangle _rectangleShape;

    private readonly PolygonShape _polygonShape;

    private readonly PhysicsBody _physicsBody;

    private readonly CircleCollider _circleCollider;

    private readonly BoxCollider _boxCollider;

    private readonly PolygonCollider _polygonCollider;

    public Cluster ()
    {
        _circleShape = AddComponent<SdfCircle> ();
        _circleShape.Color = Color.White;
        _circleShape.Offset = new Vector2 (5f, 0f);
        _circleShape.Radius = 10f;
        _circleShape.Filled = true;

        _rectangleShape = AddComponent<SdfRectangle> ();
        _rectangleShape.Color = Color.White;
        _rectangleShape.Offset = new Vector2 (0f, 25f);
        _rectangleShape.Size = new Vector2 (30f, 30f);
        _rectangleShape.Filled = true;

        _polygonShape = AddComponent<PolygonShape> ();
        _polygonShape.Offset = new Vector2 (-25f, 0f);
        _polygonShape.Color = Color.White;

        int vertexCount = 7;
        List<Vector2> vertices = new (vertexCount);

        for (int i = 0; i < vertexCount; i++)
        {
            vertices.Add (Vector2.Rotate (Vector2.UnitX * 25f, float.Tau * i / vertexCount));
        }

        _polygonShape.SetVertices (vertices);

        _physicsBody = AddComponent<PhysicsBody> ();

        _circleCollider = _physicsBody.AddCollider<CircleCollider> ();
        _circleCollider.Offset = new Vector2 (5f, 0f);
        _circleCollider.Radius = 10f;

        _boxCollider = _physicsBody.AddCollider<BoxCollider> ();
        _boxCollider.Offset = new Vector2 (0f, 25f);
        _boxCollider.Size = new Vector2 (30f, 30f);

        _polygonCollider = _physicsBody.AddCollider<PolygonCollider> ();
        _polygonCollider.Offset = new Vector2 (-25f, 0f);
        _polygonCollider.SetVertices (vertices);

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
        _circleShape.Color = Color.Red;
        _rectangleShape.Color = Color.Red;
        _polygonShape.Color = Color.Red;
    }

    private void OnCollisionStay (PhysicsBodyCollision collision)
    {
        _circleShape.Color = Color.Red;
        _rectangleShape.Color = Color.Red;
        _polygonShape.Color = Color.Red;
    }

    private void OnCollisionExit (PhysicsBodyCollision collision)
    {
        _circleShape.Color = Color.White;
        _rectangleShape.Color = Color.White;
        _polygonShape.Color = Color.White;
    }

    public override void Draw (RenderManager render)
    {
        _circleShape.Draw (render);
        _rectangleShape.Draw (render);
        _polygonShape.Draw (render);
    }
}
