namespace Eycraft.Framework;

public abstract class EntityComponent (Entity entity)
{
    public Entity Entity { get; } = entity;

    public Transform Transform => Entity.Transform;

    public virtual void Awake () { }

    public virtual void Destroy () { }
}
