namespace MonoGame.Library;

public abstract class EntityComponent (Entity owner)
{
    public Entity Owner { get; } = owner;

    public virtual void Destroy () { }
}
