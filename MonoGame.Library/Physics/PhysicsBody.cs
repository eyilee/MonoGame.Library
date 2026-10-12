using MonoGame.Library.Attributes;
using System;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

[DisallowMultipleComponent]
public class PhysicsBody : EntityComponent
{
    public enum BodyType
    {
        Dynamic,
        Kinematic,
        Static
    }

    public BodyType Type { get; set; } = BodyType.Dynamic;

    public IReadOnlyList<Collider> Colliders => _colliders.Colliders;

    public event Action<PhysicsBodyCollision>? OnCollisionEnter;

    public event Action<PhysicsBodyCollision>? OnCollisionStay;

    public event Action<PhysicsBodyCollision>? OnCollisionExit;

    private readonly ColliderContainer _colliders;

    public PhysicsBody ()
    {
        _colliders = new ColliderContainer (this);
    }

    public override void Awake ()
    {
        Transform.OnTransformChanged += OnTransformChanged;
    }

    public override void Destroy ()
    {
        Transform.OnTransformChanged -= OnTransformChanged;
    }

    private void OnTransformChanged (Transform.ChangedFlags changedFlags)
    {
        foreach (Collider collider in _colliders.Colliders)
        {
            collider.OnTransformChanged (changedFlags);
        }
    }

    public T AddCollider<T> () where T : Collider, new() => _colliders.AddCollider<T> ();

    public void RemoveCollider<T> () where T : Collider => _colliders.RemoveCollider<T> ();

    public void RemoveColliders<T> () where T : Collider => _colliders.RemoveColliders<T> ();

    public void RemoveCollider (Collider collider) => _colliders.RemoveCollider (collider);

    public bool IsDynamic => Type == BodyType.Dynamic;

    public bool IsKinematic => Type == BodyType.Kinematic;

    public bool IsStatic => Type == BodyType.Static;

    public void InvokeCollisionEnter (PhysicsBodyCollision collision) => OnCollisionEnter?.Invoke (collision);

    public void InvokeCollisionStay (PhysicsBodyCollision collision) => OnCollisionStay?.Invoke (collision);

    public void InvokeCollisionExit (PhysicsBodyCollision collision) => OnCollisionExit?.Invoke (collision);
}
