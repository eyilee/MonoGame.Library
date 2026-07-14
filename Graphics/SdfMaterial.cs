using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

public class SdfMaterial : Material
{
    private readonly EffectParameter _worldViewProjection;

    public SdfMaterial (string name, Effect effect,
        BlendState? blendState = null,
        int samplerSlot = 0,
        SamplerState? samplerState = null,
        DepthStencilState? depthStencilState = null,
        RasterizerState? rasterizerState = null,
        ushort batcherId = 0)
        : base (name, effect, blendState, samplerSlot, samplerState, depthStencilState, rasterizerState, batcherId)
    {
        _worldViewProjection = GetParameter (MaterialPropertyIds.GetId ("WorldViewProjection")) ?? throw new NullReferenceException ();
    }

    public override void OnApply ()
    {
        // TODO: modify only changed
        _worldViewProjection.SetValue (Camera.Main.GetViewProjectionMatrix ());
    }
}
