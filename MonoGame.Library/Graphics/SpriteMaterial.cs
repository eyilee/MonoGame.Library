using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public class SpriteMaterial : Material
{
    private readonly SpriteEffect _spriteEffect;

    public SpriteMaterial (string name, SpriteEffect effect, RenderBatcher renderBatcher,
        BlendState? blendState = null,
        int samplerSlot = 0,
        SamplerState? samplerState = null,
        DepthStencilState? depthStencilState = null,
        RasterizerState? rasterizerState = null)
        : base (name, effect, renderBatcher, blendState, samplerSlot, samplerState, depthStencilState, rasterizerState)
    {
        _spriteEffect = effect;
    }

    public override void OnApply ()
    {
        _spriteEffect.TransformMatrix = Camera.Main.GetViewMatrix ();
    }
}
