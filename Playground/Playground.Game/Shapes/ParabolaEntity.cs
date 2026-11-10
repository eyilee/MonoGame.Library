using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Microsoft.Xna.Framework;

namespace Playground.Game.Shapes;

public class ParabolaEntity : Entity
{
    private readonly SdfParabola _shape;

    public ParabolaEntity ()
    {
        _shape = AddComponent<SdfParabola> ();
        _shape.Color = Color.White;
        _shape.Thickness = 2f;
        _shape.Size = new Vector2 (50f, 50f);
        _shape.FocalLength = 5f;
    }

    public override void Draw (RenderManager render)
    {
        _shape.Draw (render);
    }
}
