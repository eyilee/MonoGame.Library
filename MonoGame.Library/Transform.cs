using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Library;

public class Transform (Entity owner) : EntityComponent (owner)
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
