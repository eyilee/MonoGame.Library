using Eycraft.Framework.Attributes;
using Microsoft.Xna.Framework;
using System;

namespace Eycraft.Framework;

[DisallowMultipleComponent]
[RequiredComponent]
public class Transform (Entity entity) : EntityComponent (entity)
{
    [Flags]
    public enum ChangedFlags
    {
        None = 0,
        Position = 1,
        Rotation = 2,
        All = Position | Rotation
    }

    private Vector2 _position;

    public Vector2 Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                OnTransformChanged?.Invoke (ChangedFlags.Position);
            }
        }
    }

    private float _rotation;

    public float Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                OnTransformChanged?.Invoke (ChangedFlags.Rotation);
            }
        }
    }

    public event Action<ChangedFlags>? OnTransformChanged;
}
