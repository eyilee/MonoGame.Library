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
        get { return _length; }
        set
        {
            if (_length != value)
            {
                _length = value;
                _shape.Offset = Vector2.UnitX * _length;
            }
        }
    }

    public Vector2 Target => Transform.Position + new Vector2 (float.Cos (Transform.Rotation), float.Sin (Transform.Rotation)) * _length;

    public Aiming (Entity entity) : base (entity)
    {
        _shape = Entity.AddComponent<SdfCircle> ();
        _shape.Color = Color.Red;
        _shape.Offset = Vector2.UnitX * _length;
        _shape.Radius = 3f;
        _shape.Depth = 0.5f;
    }

    public void Draw (RenderManager render)
    {
        _shape.Draw (render);
    }
}
