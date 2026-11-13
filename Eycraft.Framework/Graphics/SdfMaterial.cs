using Microsoft.Xna.Framework.Graphics;
using System;

namespace Eycraft.Framework.Graphics;

public class SdfMaterial : Material
{
    private readonly EffectParameter _worldViewProjection;

    public SdfMaterial (string name, Effect effect, RenderBatcher renderBatcher)
        : base (name, effect, renderBatcher)
    {
        _worldViewProjection = GetParameter ("WorldViewProjection") ?? throw new NullReferenceException ();
    }

    public override void OnApply ()
    {
        _worldViewProjection.SetValue (Camera.Main.GetViewProjectionMatrix ());
    }
}
