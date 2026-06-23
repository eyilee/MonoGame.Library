using System;
using System.Collections.Generic;

namespace MonoGame.Library.Graphics;

public class ResourceRegistry<T> where T : class, INamedResource
{
    private static readonly Dictionary<ushort, T> s_ids = [];

    private static readonly Dictionary<string, T> s_names = [];

    private static ushort s_nextId = 1;

    private static bool[] s_accquiredIds = new bool[32];

    internal static ushort Regist (string name, T resource)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual (s_ids.Count, ushort.MaxValue + 1);

        if (s_names.ContainsKey (name))
        {
            throw new ArgumentException ($"A resource type of '{typeof (T)}' with the name '{name}' already exists.");
        }

        ushort id = AccquireId ();

        if (s_ids.ContainsKey (id))
        {
            throw new ArgumentException ($"A resource type of '{typeof (T)}' with the id '{id}' already exists.");
        }

        s_ids.Add (id, resource);
        s_names.Add (name, resource);

        return id;
    }

    internal static void UnRegist (ushort id)
    {
        if (!s_ids.TryGetValue (id, out T? resource))
        {
            return;
        }

        s_ids.Remove (id);
        s_names.Remove (resource.Name);

        ReleaseId (id);
    }

    internal static void UnRegist (string name)
    {
        if (!s_names.TryGetValue (name, out T? resource))
        {
            return;
        }

        s_ids.Remove (resource.Id);
        s_names.Remove (name);

        ReleaseId (resource.Id);
    }

    internal static void UnRegist (T resource)
    {
        if (!s_ids.TryGetValue (resource.Id, out T? idResource))
        {
            return;
        }

        if (!s_names.TryGetValue (resource.Name, out T? nameResource))
        {
            return;
        }

        if (!ReferenceEquals (resource, idResource) || !ReferenceEquals (resource, nameResource))
        {
            return;
        }

        s_ids.Remove (resource.Id);
        s_names.Remove (resource.Name);
    }

    public static bool TryGetValue (ushort id, out T? resource) => s_ids.TryGetValue (id, out resource);

    public static bool TryGetValue (string name, out T? resource) => s_names.TryGetValue (name, out resource);

    public static ushort GetId (string name)
    {
        if (s_names.TryGetValue (name, out T? resource))
        {
            return resource.Id;
        }

        return 0;
    }

    public static string GetName (ushort id)
    {
        if (s_ids.TryGetValue (id, out T? resource))
        {
            return resource.Name;
        }

        return string.Empty;
    }

    private static ushort AccquireId ()
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual (s_ids.Count, ushort.MaxValue + 1);

        unchecked
        {
            while (s_nextId == 0 || s_accquiredIds[s_nextId])
            {
                s_nextId++;

                if (s_nextId >= s_accquiredIds.Length)
                {
                    Array.Resize (ref s_accquiredIds, s_accquiredIds.Length * 2);
                }
            }
        }

        s_accquiredIds[s_nextId] = true;

        return s_nextId;
    }

    private static void ReleaseId (ushort id)
    {
        if (id >= s_accquiredIds.Length)
        {
            return;
        }

        s_accquiredIds[id] = false;
    }
}
