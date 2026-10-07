namespace MonoGame.Library.Physics;

public class PhysicsBody : EntityComponent
{
    public Collider? Collider { get; private set; }

    public bool IsStatic { get; set; }

    public override void Awake ()
    {
        Transform.OnTransformChanged += OnTransformChanged;
    }

    public override void Destroy ()
    {
        Transform.OnTransformChanged -= OnTransformChanged;
    }

    private void OnTransformChanged ()
    {
        if (Collider != null)
        {
            Collider.Position = Transform.Position;
            Collider.Rotation = Transform.Rotation;
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
