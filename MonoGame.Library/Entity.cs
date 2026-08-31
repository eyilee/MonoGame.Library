using Microsoft.Xna.Framework;
using MonoGame.Library.Physics;

namespace MonoGame.Library;

public class Entity
{
    public Transform Transform => _transform;

    public virtual Vector2 Position
    {
        get => _transform.Position;
        set => _transform.Position = value;
    }

    public virtual float Rotation
    {
        get => _transform.Rotation;
        set => _transform.Rotation = value;
    }

    public PhysicsBody? PhysicsBody => _physicsBody;

    private readonly Transform _transform;

    private PhysicsBody? _physicsBody;

    public Entity ()
    {
        _transform = new Transform (this);
        _transform.OnTransformChanged += OnTransformChanged;
    }

    public void AddPhysics (PhysicsWorld physicsWorld)
    {
        if (_physicsBody != null)
        {
            return;
        }

        _physicsBody = new PhysicsBody (this);

        physicsWorld.Add (_physicsBody);
    }

    public void RemovePhysics (PhysicsWorld physicsWorld)
    {
        if (_physicsBody == null)
        {
            return;
        }

        physicsWorld.Remove (_physicsBody);

        _physicsBody.Destroy ();
        _physicsBody = null;
    }

    public virtual void OnTransformChanged () { }

    public virtual void OnCollisionEnter (Collision collision) { }

    public virtual void OnCollisionStay (Collision collision) { }

    public virtual void OnCollisionExit (Collision collision) { }
}
