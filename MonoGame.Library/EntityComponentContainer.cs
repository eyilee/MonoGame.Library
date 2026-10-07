using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Library;

public sealed class EntityComponentContainer (Entity owner)
{
    private readonly Entity _entity = owner;

    private readonly Dictionary<Type, List<EntityComponent>> _components = [];

    public EntityComponent? GetComponent (Type type)
    {
        if (!_components.TryGetValue (type, out List<EntityComponent>? components) || components.Count == 0)
        {
            return null;
        }

        return components[0];
    }

    public T? GetComponent<T> () where T : EntityComponent
    {
        return GetComponent (typeof (T)) as T;
    }

    public List<EntityComponent> GetComponents (Type type)
    {
        if (!_components.TryGetValue (type, out List<EntityComponent>? components) || components.Count == 0)
        {
            return [];
        }

        return components;
    }

    public List<T> GetComponents<T> () where T : EntityComponent
    {
        return GetComponents (typeof (T)).Cast<T> ().ToList ();
    }

    public EntityComponent AddComponent (Type type)
    {
        if (!_components.TryGetValue (type, out List<EntityComponent>? components))
        {
            components = [];
            _components[type] = components;
        }

        if (EntityComponentTypeInfo.DisallowMultiple (type))
        {
            if (components.Count > 0)
            {
                throw new InvalidOperationException ($"Cannot add multiple components of type {type.Name}.");
            }
        }

        AddRequireComponents (type);

        if (Activator.CreateInstance (type) is not EntityComponent component)
        {
            throw new InvalidOperationException ($"Type {type.Name} is not a valid EntityComponent.");
        }

        component.Entity = _entity;
        components.Add (component);
        component.Awake ();

        return component;
    }

    public T AddComponent<T> () where T : EntityComponent, new()
    {
        return (T)AddComponent (typeof (T));
    }

    private void AddRequireComponents (Type type)
    {
        foreach (Type requireComponentType in EntityComponentTypeInfo.RequireComponents (type))
        {
            if (GetComponent (type) == null)
            {
                AddComponent (requireComponentType);
            }
        }
    }

    public void RemoveComponent (Type type)
    {
        if (!_components.TryGetValue (type, out List<EntityComponent>? components) || components.Count == 0)
        {
            return;
        }

        EnsureRemoveComponent (type);

        EntityComponent component = components[0];
        component.Destroy ();
        components.Remove (component);
    }

    public void RemoveComponent<T> () where T : EntityComponent
    {
        RemoveComponent (typeof (T));
    }

    public void RemoveComponents (Type type)
    {
        if (!_components.TryGetValue (type, out List<EntityComponent>? components) || components.Count == 0)
        {
            return;
        }

        EnsureRemoveComponent (type);

        foreach (EntityComponent component in components)
        {
            component.Destroy ();
        }

        components.Clear ();
    }

    public void RemoveComponents<T> () where T : EntityComponent
    {
        RemoveComponents (typeof (T));
    }

    public void RemoveComponent (EntityComponent component)
    {
        Type type = component.GetType ();

        if (!_components.TryGetValue (type, out List<EntityComponent>? components) || components.Count == 0)
        {
            return;
        }

        EnsureRemoveComponent (type);

        if (components.Contains (component))
        {
            component.Destroy ();
            components.Remove (component);
        }
    }

    private void EnsureRemoveComponent (Type type)
    {
        if (EntityComponentTypeInfo.RequiredComponent (type))
        {
            throw new InvalidOperationException ($"Cannot remove required component of type {type.Name}.");
        }

        foreach ((Type componentType, List<EntityComponent> b) in _components)
        {
            if (componentType == type)
            {
                continue;
            }

            foreach (Type requireComponentType in EntityComponentTypeInfo.RequireComponents (componentType))
            {
                if (requireComponentType == type)
                {
                    throw new InvalidOperationException ($"Cannot remove component of type {type.Name} because it is required by component of type {componentType.Name}.");
                }
            }
        }
    }
}
