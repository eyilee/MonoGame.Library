namespace MonoGame.Library;

public abstract class EntityComponent
{
    public Entity Entity { get; internal set; } = null!;

    public Transform Transform => Entity.Transform;

    public virtual void Awake () { }

    public virtual void Destroy () { }
}
