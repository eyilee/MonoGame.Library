using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Microsoft.Xna.Framework;

namespace Playground.Game.Shapes;

public class Origin : Entity
{
    private readonly SdfCircle _shape;

    public Origin ()
    {
        _shape = AddComponent<SdfCircle> ();
        _shape.Thickness = 2f;
        _shape.Color = Color.Green;
        _shape.Radius = 5f;
        _shape.SortingLayer = 1;
    }

    public override void Draw (RenderManager render)
    {
        _shape.Draw (render);
    }
}
