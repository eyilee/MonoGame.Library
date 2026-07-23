using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGame.Library.Graphics;

public class Material : ResourceRegistry<Material>, IResource, IDisposable
{
    public ushort Id { get; }

    public string Name { get; }

    public Effect Effect { get; }

    public BlendState BlendState { get; }

    public int SamplerSlot { get; }

    public SamplerState SamplerState { get; }

    public DepthStencilState DepthStencilState { get; }

    public RasterizerState RasterizerState { get; }

    public RenderBatcher RenderBatcher { get; }

    public MaterialPropertyBlock PropertyBlock { get; } = new ();

    private readonly Dictionary<int, EffectParameter?> _parameters = [];

    private bool _disposed;

    public Material (string name, Effect effect, RenderBatcher renderBatcher,
        BlendState? blendState = null,
        int samplerSlot = 0,
        SamplerState? samplerState = null,
        DepthStencilState? depthStencilState = null,
        RasterizerState? rasterizerState = null)
    {
        Id = Regist (name, this);
        Name = name;
        Effect = effect;
        RenderBatcher = renderBatcher;
        BlendState = blendState ?? BlendState.AlphaBlend;
        SamplerSlot = samplerSlot;
        SamplerState = samplerState ?? SamplerState.LinearClamp;
        DepthStencilState = depthStencilState ?? DepthStencilState.None;
        RasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;

        foreach (EffectParameter? parameter in Effect.Parameters)
        {
            _parameters[MaterialPropertyIds.GetId (parameter.Name)] = parameter;
        }
    }

    ~Material () => Dispose (false);

    public EffectParameter? GetParameter (int propertyId)
    {
        if (_parameters.TryGetValue (propertyId, out EffectParameter? parameter))
        {
            return parameter;
        }

        parameter = MaterialPropertyIds.TryGetName (propertyId, out string name) ? Effect.Parameters[name] : null;
        _parameters[propertyId] = parameter;

        return parameter;
    }

    public virtual void OnApply () { }

    public void ApplyStates (GraphicsDevice graphicsDevice)
    {
        ArgumentNullException.ThrowIfNull (graphicsDevice);

        graphicsDevice.BlendState = BlendState;
        graphicsDevice.DepthStencilState = DepthStencilState;
        graphicsDevice.RasterizerState = RasterizerState;
        graphicsDevice.SamplerStates[SamplerSlot] = SamplerState;
    }

    public void ApplyProperties (MaterialPropertyBlock? propertyBlock = null)
    {
        OnApply ();

        PropertyBlock.ApplyTo (this);
        propertyBlock?.ApplyTo (this);
    }

    public void Dispose ()
    {
        Dispose (true);
        GC.SuppressFinalize (this);
    }

    protected virtual void Dispose (bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                UnRegist (this);
            }

            _disposed = true;
        }
    }
}
