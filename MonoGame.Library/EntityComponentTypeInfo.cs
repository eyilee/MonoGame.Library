using MonoGame.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Library;

public static class EntityComponentTypeInfo<T> where T : EntityComponent
{
    public static readonly bool DisallowMultiple = typeof (T).IsDefined (typeof (DisallowMultipleComponentAttribute), true);

    public static readonly Type[] RequireComponents = typeof (T).GetCustomAttributes (typeof (RequireComponentAttribute), true)
        .Cast<RequireComponentAttribute> ()
        .Select (x => x.ComponentType)
        .ToArray ();

    public static readonly bool RequiredComponent = typeof (T).IsDefined (typeof (RequiredComponentAttribute), true);
}

public static class EntityComponentTypeInfo
{
    private readonly static Dictionary<Type, bool> _disallowMultipleCache = [];

    private readonly static Dictionary<Type, Type[]> _requireComponentsCache = [];

    private readonly static Dictionary<Type, bool> _requiredComponentCache = [];

    public static bool DisallowMultiple (Type type)
    {
        if (!_disallowMultipleCache.TryGetValue (type, out bool disallowMultiple))
        {
            disallowMultiple = type.IsDefined (typeof (DisallowMultipleComponentAttribute), true);
            _disallowMultipleCache[type] = disallowMultiple;
        }

        return disallowMultiple;
    }

    public static Type[] RequireComponents (Type type)
    {
        if (!_requireComponentsCache.TryGetValue (type, out Type[]? requireComponents))
        {
            requireComponents = type.GetCustomAttributes (typeof (RequireComponentAttribute), true)
                .Cast<RequireComponentAttribute> ()
                .Select (x => x.ComponentType)
                .ToArray ();

            _requireComponentsCache[type] = requireComponents;
        }

        return requireComponents;
    }

    public static bool RequiredComponent (Type type)
    {
        if (!_requiredComponentCache.TryGetValue (type, out bool requiredComponent))
        {
            requiredComponent = type.IsDefined (typeof (DisallowMultipleComponentAttribute), true);
            _requiredComponentCache[type] = requiredComponent;
        }

        return requiredComponent;
    }
}
