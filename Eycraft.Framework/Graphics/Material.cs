using Eycraft.Framework.Resource;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Eycraft.Framework.Graphics;

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

    #region Built-in Materials
    private static Material? _standard;

    public static Material Standard
    {
        get
        {
            _standard ??= new SpriteMaterial ("Standard", new SpriteEffect (Core.GraphicsDevice), RenderBatchers.Standard);

            return _standard;
        }
    }

    private static Material? _sprite;

    public static Material Sprite
    {
        get
        {
            _sprite ??= new SpriteMaterial ("Sprite", new SpriteEffect (Core.GraphicsDevice), RenderBatchers.Sprite, samplerState: SamplerState.PointClamp);

            return _sprite;
        }
    }

    private static Material? _spriteFont;

    public static Material SpriteFont
    {
        get
        {
            _spriteFont ??= new SpriteMaterial ("SpriteFont", new SpriteEffect (Core.GraphicsDevice), RenderBatchers.Standard, samplerState: SamplerState.PointClamp);

            return _spriteFont;
        }
    }

    private static SdfMaterial? _sdfCircle;

    public static SdfMaterial SdfCircle
    {
        get
        {
            _sdfCircle ??= new SdfMaterial ("SdfCircle", EffectResource.SdfCircle.Effect, RenderBatchers.SdfInstance);

            return _sdfCircle;
        }
    }

    private static SdfMaterial? _sdfFilledCircle;

    public static SdfMaterial SdfFilledCircle
    {
        get
        {
            _sdfFilledCircle ??= new SdfMaterial ("SdfFilledCircle", EffectResource.SdfFilledCircle.Effect, RenderBatchers.SdfInstance);

            return _sdfFilledCircle;
        }
    }

    private static SdfMaterial? _sdfLine;

    public static SdfMaterial SdfLine
    {
        get
        {
            _sdfLine ??= new SdfMaterial ("SdfLine", EffectResource.SdfLine.Effect, RenderBatchers.SdfInstance);

            return _sdfLine;
        }
    }

    private static SdfMaterial? _sdfParabola;

    public static SdfMaterial SdfParabola
    {
        get
        {
            _sdfParabola ??= new SdfMaterial ("SdfParabola", EffectResource.SdfParabola.Effect, RenderBatchers.SdfInstance);

            return _sdfParabola;
        }
    }

    private static SdfMaterial? _sdfRectangle;

    public static SdfMaterial SdfRectangle
    {
        get
        {
            _sdfRectangle ??= new SdfMaterial ("SdfRectangle", EffectResource.SdfRectangle.Effect, RenderBatchers.SdfInstance);

            return _sdfRectangle;
        }
    }

    private static SdfMaterial? _sdfFilledRectangle;

    public static SdfMaterial SdfFilledRectangle
    {
        get
        {
            _sdfFilledRectangle ??= new SdfMaterial ("SdfFilledRectangle", EffectResource.SdfFilledRectangle.Effect, RenderBatchers.SdfInstance);

            return _sdfFilledRectangle;
        }
    }
    #endregion
}
