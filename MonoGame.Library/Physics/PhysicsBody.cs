namespace MonoGame.Library.Physics;

public class PhysicsBody : EntityComponent
{
    public Collider Collider { get; } = new ();

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
        Collider.Position = Owner.Position;
        Collider.Rotation = Owner.Rotation;
    }
}
