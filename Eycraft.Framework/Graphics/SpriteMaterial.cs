using Microsoft.Xna.Framework.Graphics;

namespace Eycraft.Framework.Graphics;

public class SpriteMaterial : Material
{
    private readonly SpriteEffect _spriteEffect;

    public SpriteMaterial (string name, SpriteEffect effect, RenderBatcher renderBatcher)
        : base (name, effect, renderBatcher)
    {
        SamplerState = SamplerState.PointClamp;

        _spriteEffect = effect;
    }

    public override void OnApply ()
    {
        _spriteEffect.TransformMatrix = Camera.Main.GetViewMatrix ();
    }
}
