namespace MonoGame.Library.Physics;

public class Collision (Collider colliderA, Collider colliderB)
{
    public Collider ColliderA { get; } = colliderA;

    public Collider ColliderB { get; } = colliderB;
}
