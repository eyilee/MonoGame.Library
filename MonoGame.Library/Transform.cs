using Microsoft.Xna.Framework;
using MonoGame.Library.Attributes;
using System;

namespace MonoGame.Library;

[DisallowMultipleComponent]
[RequiredComponent]
public class Transform : EntityComponent
{
    public Vector2 Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                OnTransformChanged?.Invoke ();
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
                OnTransformChanged?.Invoke ();
            }
        }
    }

    public event Action? OnTransformChanged;

    private Vector2 _position;

    private float _rotation;
}
