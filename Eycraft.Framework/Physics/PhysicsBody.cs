using Eycraft.Framework.Attributes;
using System;
using System.Collections.Generic;

namespace Eycraft.Framework.Physics;

[DisallowMultipleComponent]
public class PhysicsBody : EntityComponent, IDisposable
{
    public enum BodyType
    {
        Dynamic,
        Kinematic,
        Static
    }

    private BodyType _bodyType = BodyType.Dynamic;

    public BodyType Type
    {
        get => _bodyType;
        set
        {
            if (_bodyType != value)
            {
                _bodyType = value;
                _physicsWorld?.SetDirty (this);
            }
        }
    }

    public bool IsDynamic => Type == BodyType.Dynamic;

    public bool IsKinematic => Type == BodyType.Kinematic;

    public bool IsStatic => Type == BodyType.Static;

    public IReadOnlySet<Collider> Colliders => _colliders.Colliders;

    public event Action<Collision>? OnCollisionEnter;

    public event Action<Collision>? OnCollisionStay;

    public event Action<Collision>? OnCollisionExit;

    private readonly ColliderContainer _colliders;

    private PhysicsWorld? _physicsWorld;

    private bool _disposed;

    public PhysicsBody (Entity entity) : base (entity)
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

    public void AttachPhysics (PhysicsWorld physicsWorld)
    {
        _physicsWorld?.Remove (this);
        _physicsWorld = physicsWorld;
        _physicsWorld.Add (this);
    }

    public void DetachPhysics ()
    {
        _physicsWorld?.Remove (this);
        _physicsWorld = null;
    }

    internal void SetDirty (Collider collider)
    {
        if (IsStatic)
        {
            _physicsWorld?.SetDirty (collider);
        }
    }

    public T AddCollider<T> () where T : Collider
    {
        T collider = _colliders.AddCollider<T> ();

        SetDirty (collider);

        return collider;
    }

    public void RemoveCollider (Collider collider)
    {
        _colliders.RemoveCollider (collider);

        SetDirty (collider);
    }

    public void InvokeCollisionEnter (Collision collision) => OnCollisionEnter?.Invoke (collision);

    public void InvokeCollisionStay (Collision collision) => OnCollisionStay?.Invoke (collision);

    public void InvokeCollisionExit (Collision collision) => OnCollisionExit?.Invoke (collision);

    public virtual void Dispose ()
    {
        if (!_disposed)
        {
            DetachPhysics ();

            _disposed = true;
        }

        GC.SuppressFinalize (this);
    }
}
