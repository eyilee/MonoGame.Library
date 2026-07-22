using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

public class MaterialInstance (Material material)
{
    public Material Material { get; } = material;

    public ushort Id => Material.Id;

    public string Name => Material.Name;

    public Effect Effect => Material.Effect;

    public BlendState BlendState
    {
        get => _blendState ?? Material.BlendState;
        set => _blendState = value;
    }

    public int SamplerSlot => Material.SamplerSlot;

    public SamplerState SamplerState
    {
        get => _samplerState ?? Material.SamplerState;
        set => _samplerState = value;
    }

    public DepthStencilState DepthStencilState
    {
        get => _depthStencilState ?? Material.DepthStencilState;
        set => _depthStencilState = value;
    }

    public RasterizerState RasterizerState
    {
        get => _rasterizerState ?? Material.RasterizerState;
        set => _rasterizerState = value;
    }

    public ushort BatcherId => Material.BatcherId;

    public EffectParameter? GetParameter (int propertyId) => Material.GetParameter (propertyId);

    public MaterialPropertyBlock PropertyBlock { get; } = new ();

    private BlendState? _blendState;

    private SamplerState? _samplerState;

    private DepthStencilState? _depthStencilState;

    private RasterizerState? _rasterizerState;

    public void ApplyStates (GraphicsDevice graphicsDevice)
    {
        ArgumentNullException.ThrowIfNull (graphicsDevice);

        graphicsDevice.BlendState = _blendState ?? Material.BlendState;
        graphicsDevice.DepthStencilState = _depthStencilState ?? Material.DepthStencilState;
        graphicsDevice.RasterizerState = _rasterizerState ?? Material.RasterizerState;
        graphicsDevice.SamplerStates[Material.SamplerSlot] = _samplerState ?? Material.SamplerState;
    }

    public void ApplyProperties (MaterialPropertyBlock? propertyBlock = null)
    {
        Material.OnApply ();

        PropertyBlock.ApplyTo (Material);
        propertyBlock?.ApplyTo (Material);
    }
}
