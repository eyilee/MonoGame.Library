using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Playground.Game;

public class Cube : Entity
{
    private struct SubCube
    {
        public SdfRectangle Shape;

        public BoxCollider Collider;
    }

    private PhysicsBody _physicsBody = null!;

    private readonly SdfCircle _shape = new ();

    private readonly List<SubCube> _subCubes = [];

    private bool _initialized = false;

    public void Initialize (Vector2 position, float rotation)
    {
        if (_initialized)
        {
            return;
        }

        Transform.OnTransformChanged += OnTransformChanged;
        Transform.Position = position;
        Transform.Rotation = rotation;

        _physicsBody = AddComponent<PhysicsBody> ();

        _shape.Color = Color.White;
        _shape.Rotation = rotation;
        _shape.Thickness = 1f;
        _shape.Position = position;
        _shape.Radius = 5f;

        for (int i = 0; i < 4; i++)
        {
            Vector2 offset = Vector2.Rotate (Vector2.UnitX * 20f, float.Pi * 2f * i / 4f);
            Vector2 size = new (10f, 10f);

            SdfRectangle shape = new ()
            {
                Color = Color.White,
                Thickness = 3f,
                Position = Transform.Position + offset,
                Size = size
            };

            BoxCollider collider = _physicsBody.AddCollider<BoxCollider> ();
            collider.Offset = offset;
            collider.Size = size;

            _subCubes.Add (new SubCube
            {
                Shape = shape,
                Collider = collider
            });
        }

        _physicsBody.OnCollisionEnter += OnCollisionEnter;
        _physicsBody.OnCollisionStay += OnCollisionStay;
        _physicsBody.OnCollisionExit += OnCollisionExit;

        _initialized = true;
    }

    private void OnTransformChanged (Transform.ChangedFlags changedFlags)
    {
        _shape.Position = Transform.Position;
        _shape.Rotation = Transform.Rotation;

        for (int i = 0; i < 4; i++)
        {
            Vector2 offset = Vector2.Rotate (Vector2.UnitX * 20f, float.Pi * 2f * i / 4f);

            SdfRectangle shape = _subCubes[i].Shape;
            shape.Position = Transform.Position + Vector2.Rotate (offset, Transform.Rotation);
            shape.Rotation = Transform.Rotation;
        }
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
        foreach (SubCube subCube in _subCubes)
        {
            subCube.Shape.Color = Color.Red;
        }
    }

    private void OnCollisionStay (PhysicsBodyCollision collision)
    {
        foreach (SubCube subCube in _subCubes)
        {
            subCube.Shape.Color = Color.Red;
        }
    }

    private void OnCollisionExit (PhysicsBodyCollision collision)
    {
        foreach (SubCube subCube in _subCubes)
        {
            subCube.Shape.Color = Color.White;
        }
    }

    public override void Draw (RenderManager render)
    {
        if (!_initialized)
        {
            return;
        }

        foreach (SubCube subCube in _subCubes)
        {
            subCube.Shape.Draw (render);
        }

        _shape.Draw (render);
    }
}
