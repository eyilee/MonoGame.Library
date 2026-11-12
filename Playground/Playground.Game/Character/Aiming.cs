using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Microsoft.Xna.Framework;

namespace Playground.Game.Character;

public class Aiming : EntityComponent
{
    private readonly SdfCircle _shape;

    private float _length = 50f;

    public float Length
    {
        get => _length;
        set
        {
            if (_length != value)
            {
                _length = value;
                _dirty = true;
            }
        }
    }

    private float _rotation = 0f;

    public float Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                _dirty = true;
            }
        }
    }

    private bool _dirty = true;

    public Vector2 Target => Transform.Position + new Vector2 (float.Cos (Rotation), float.Sin (Rotation)) * Length;

    public Aiming (Entity entity) : base (entity)
    {
        _shape = Entity.AddComponent<SdfCircle> ();
        _shape.Color = Color.Red;
        _shape.Radius = 3f;
        _shape.Depth = 0.5f;
    }

    public void Draw (RenderManager render)
    {
        if (_dirty)
        {
            _shape.Offset = Vector2.Rotate (Vector2.UnitX, Rotation) * Length;
            _dirty = false;
        }

        _shape.Draw (render);
    }
}
