namespace MonoGame.Library.Physics;

public class PhysicsEntity : Entity
{
    public Collider Collider { get; }

    public PhysicsEntity ()
    {
        Collider = new Collider (this);
    }
}
