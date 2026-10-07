using Microsoft.Xna.Framework;
using MonoGame.Library.Physics;
using System;
using System.Collections.Generic;

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

    private readonly EntityComponentContainer _components;

    public Entity ()
    {
        _components = new EntityComponentContainer (this);

        _transform = AddComponent<Transform> ();
        _transform.OnTransformChanged += OnTransformChanged;
    }

    public void AddPhysics (PhysicsWorld physicsWorld)
    {
        if (_physicsBody != null)
        {
            return;
        }

        _physicsBody = AddComponent<PhysicsBody> ();

        physicsWorld.Add (_physicsBody);
    }

    public void RemovePhysics (PhysicsWorld physicsWorld)
    {
        if (_physicsBody == null)
        {
            return;
        }

        physicsWorld.Remove (_physicsBody);

        RemoveComponent (_physicsBody);
        _physicsBody = null;
    }

    public virtual void OnTransformChanged () { }

    public virtual void OnCollisionEnter (Collision collision) { }

    public virtual void OnCollisionStay (Collision collision) { }

    public virtual void OnCollisionExit (Collision collision) { }

    public EntityComponent? GetComponent (Type type) => _components.GetComponent (type);

    public T? GetComponent<T> () where T : EntityComponent => _components.GetComponent<T> ();

    public List<EntityComponent> GetComponents (Type type) => _components.GetComponents (type);

    public List<T> GetComponents<T> () where T : EntityComponent => _components.GetComponents<T> ();

    public EntityComponent AddComponent (Type type) => _components.AddComponent (type);

    public T AddComponent<T> () where T : EntityComponent, new() => _components.AddComponent<T> ();

    public void RemoveComponent (Type type) => _components.RemoveComponent (type);

    public void RemoveComponent<T> () where T : EntityComponent => _components.RemoveComponent<T> ();

    public void RemoveComponents (Type type) => _components.RemoveComponents (type);

    public void RemoveComponents<T> () where T : EntityComponent => _components.RemoveComponents<T> ();

    public void RemoveComponent (EntityComponent component) => _components.RemoveComponent (component);
}
