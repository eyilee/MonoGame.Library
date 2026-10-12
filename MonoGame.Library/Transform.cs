using Microsoft.Xna.Framework;
using MonoGame.Library.Attributes;
using System;

namespace MonoGame.Library;

[DisallowMultipleComponent]
[RequiredComponent]
public class Transform : EntityComponent
{
    [Flags]
    public enum ChangedFlags
    {
        None = 0,
        Position = 1,
        Rotation = 2,
        All = Position | Rotation
    }

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

    private Vector2 _position;

    private float _rotation;
}
