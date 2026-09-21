namespace MonoGame.Library.Physics;

public class PhysicsBody : EntityComponent
{
    public Collider? Collider { get; private set; }

    public bool IsStatic { get; set; }

    public PhysicsBody (Entity owner) : base (owner)
    {
        Owner.Transform.OnTransformChanged += OnTransformChanged;
    }

    public override void Destroy ()
    {
        Owner.Transform.OnTransformChanged -= OnTransformChanged;
    }

    public virtual void OnTransformChanged ()
    {
        if (Collider != null)
        {
            Collider.Position = Owner.Position;
            Collider.Rotation = Owner.Rotation;
        }
    }

    public void AttachCollider (Collider collider)
    {
        Collider = collider;
        OnTransformChanged ();
    }

    public void DetachCollider ()
    {
        Collider = null;
    }
}
