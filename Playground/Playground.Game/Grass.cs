using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;

namespace Playground.Game;

public class Grass : Entity
{
    private readonly Sprite _sprite;

    public Grass (TextureRegion region)
    {
        _sprite = AddComponent<Sprite> ();
        _sprite.OriginAlignment = OriginAlignment.Center;
        _sprite.Region = region;
    }

    public override void Draw (RenderManager render)
    {
        _sprite.Draw (render);
    }
}
