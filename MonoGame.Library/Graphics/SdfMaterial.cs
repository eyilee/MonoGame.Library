using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

public class SdfMaterial : Material
{
    private readonly EffectParameter _worldViewProjection;

    public SdfMaterial (string name, Effect effect, RenderBatcher renderBatcher,
        BlendState? blendState = null,
        int samplerSlot = 0,
        SamplerState? samplerState = null,
        DepthStencilState? depthStencilState = null,
        RasterizerState? rasterizerState = null)
        : base (name, effect, renderBatcher, blendState, samplerSlot, samplerState, depthStencilState, rasterizerState)
    {
        _worldViewProjection = GetParameter (MaterialPropertyIds.GetId ("WorldViewProjection")) ?? throw new NullReferenceException ();
    }

    public override void OnApply ()
    {
        // TODO: modify only changed
        _worldViewProjection.SetValue (Camera.Main.GetViewProjectionMatrix ());
    }
}
