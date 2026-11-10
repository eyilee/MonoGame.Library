using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Eycraft.Framework.Physics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Playground.Game.Character;

public class Aiming : EntityComponent
{
    private readonly SdfCircle _shape;

    private readonly float _length = 50f;

    public Vector2 Target => Transform.Position + new Vector2 (float.Cos (Transform.Rotation), float.Sin (Transform.Rotation)) * _length;

    public Aiming (Entity entity) : base (entity)
    {
        _shape = Entity.AddComponent<SdfCircle> ();
        _shape.Color = Color.Red;
        _shape.Offset = Vector2.UnitX * _length;
        _shape.Radius = 3f;
    }

    public void Draw (RenderManager render)
    {
        _shape.Draw (render);
    }
}
