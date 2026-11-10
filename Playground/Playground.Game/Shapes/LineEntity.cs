using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Microsoft.Xna.Framework;

namespace Playground.Game.Shapes;

public class LineEntity : Entity
{
    private readonly SdfLine _shape;

    public LineEntity ()
    {
        _shape = AddComponent<SdfLine> ();
        _shape.Color = Color.White;
        _shape.Thickness = 2f;
        _shape.Start = new Vector2 (5f, 5f);
        _shape.End = new Vector2 (-5f, -5f);
    }

    public override void Draw (RenderManager render)
    {
        _shape.Draw (render);
    }
}
