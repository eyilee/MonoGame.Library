using System;
using System.Collections.Generic;

namespace MonoGame.Library;

public class Entity
{
    private readonly EntityComponentContainer _components;

    public Transform Transform { get; }

    public Entity ()
    {
        _components = new EntityComponentContainer (this);

        Transform = AddComponent<Transform> ();
    }

    public EntityComponent? GetComponent (Type type) => _components.GetComponent (type);

    public T? GetComponent<T> () where T : EntityComponent => _components.GetComponent<T> ();

    public List<EntityComponent> GetComponents (Type type) => _components.GetComponents (type);

    public List<T> GetComponents<T> () where T : EntityComponent => _components.GetComponents<T> ();

    public EntityComponent AddComponent (Type type) => _components.AddComponent (type);

    public T AddComponent<T> () where T : EntityComponent, new() => _components.AddComponent<T> ();

    public void RemoveComponent (Type type) => _components.RemoveComponent (type);

    public void RemoveComponent<T> () where T : EntityComponent => _components.RemoveComponent<T> ();

    public void RemoveComponents (Type type) => _components.RemoveComponents (type);

    public void RemoveComponents<T> () where T : EntityComponent => _components.RemoveComponents<T> ();

    public void RemoveComponent (EntityComponent component) => _components.RemoveComponent (component);
}
