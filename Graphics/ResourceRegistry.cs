using System;
using System.Collections.Generic;

namespace MonoGame.Library.Graphics;

public class ResourceRegistry<T> where T : class, INamedResource
{
    private readonly Dictionary<ushort, T> _ids = [];

    private readonly Dictionary<string, T> _names = [];

    private ushort _nextId = 1;

    private bool[] _accquiredIds = new bool[32];

    internal ushort Regist (string name, T resource)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual (_ids.Count, ushort.MaxValue + 1);

        if (_names.ContainsKey (name))
        {
            throw new ArgumentException ($"A resource type of '{typeof (T)}' with the name '{name}' already exists.");
        }

        ushort id = AccquireId ();

        if (_ids.ContainsKey (id))
        {
            throw new ArgumentException ($"A resource type of '{typeof (T)}' with the id '{id}' already exists.");
        }

        _ids.Add (id, resource);
        _names.Add (name, resource);

        return id;
    }

    internal void UnRegist (ushort id)
    {
        if (!_ids.TryGetValue (id, out T? resource))
        {
            return;
        }

        _ids.Remove (id);
        _names.Remove (resource.Name);

        ReleaseId (id);
    }

    internal void UnRegist (string name)
    {
        if (!_names.TryGetValue (name, out T? resource))
        {
            return;
        }

        _ids.Remove (resource.Id);
        _names.Remove (name);

        ReleaseId (resource.Id);
    }

    internal void UnRegist (T resource)
    {
        if (!_ids.TryGetValue (resource.Id, out T? idResource))
        {
            return;
        }

        if (!_names.TryGetValue (resource.Name, out T? nameResource))
        {
            return;
        }

        if (!ReferenceEquals (resource, idResource) || !ReferenceEquals (resource, nameResource))
        {
            return;
        }

        _ids.Remove (resource.Id);
        _names.Remove (resource.Name);
    }

    public bool TryGetValue (ushort id, out T? resource) => _ids.TryGetValue (id, out resource);

    public bool TryGetValue (string name, out T? resource) => _names.TryGetValue (name, out resource);

    public ushort GetId (string name)
    {
        if (_names.TryGetValue (name, out T? resource))
        {
            return resource.Id;
        }

        return 0;
    }

    public string GetName (ushort id)
    {
        if (_ids.TryGetValue (id, out T? resource))
        {
            return resource.Name;
        }

        return string.Empty;
    }

    private ushort AccquireId ()
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual (_ids.Count, ushort.MaxValue + 1);

        unchecked
        {
            while (_nextId == 0 || _accquiredIds[_nextId])
            {
                _nextId++;

                if (_nextId >= _accquiredIds.Length)
                {
                    Array.Resize (ref _accquiredIds, _accquiredIds.Length * 2);
                }
            }
        }

        _accquiredIds[_nextId] = true;

        return _nextId;
    }

    private void ReleaseId (ushort id)
    {
        if (id >= _accquiredIds.Length)
        {
            return;
        }

        _accquiredIds[id] = false;
    }
}
