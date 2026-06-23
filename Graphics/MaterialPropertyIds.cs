using System;
using System.Collections.Generic;

namespace MonoGame.Library.Graphics;

public static class MaterialPropertyIds
{
    private static readonly Dictionary<string, int> s_nameToId = [];

    private static readonly Dictionary<int, string> s_idToName = [];

    private static int s_nextId = 0;

    public static int GetId (string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace (name);

        if (s_nameToId.TryGetValue (name, out int id))
        {
            return id;
        }

        id = s_nextId++;

        s_nameToId.Add (name, id);
        s_idToName.Add (id, name);

        return id;
    }

    public static bool TryGetName (int id, out string name) => s_idToName.TryGetValue (id, out name!);
}
