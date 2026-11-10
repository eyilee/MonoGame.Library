using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;

namespace Playground.Game;

public class Grass : Entity
{
    private readonly Sprite _sprite;

    public Grass (TextureRegion region)
    {
        _sprite = new ()
        {
            Region = region
        };

        Transform.OnTransformChanged += OnTransformChanged;
    }

    private void OnTransformChanged (Transform.ChangedFlags changedFlags)
    {
        _sprite.Position = Transform.Position;
        _sprite.Rotation = Transform.Rotation;
    }

    public override void Draw (RenderManager render)
    {
        _sprite.Draw (render);
    }
}
