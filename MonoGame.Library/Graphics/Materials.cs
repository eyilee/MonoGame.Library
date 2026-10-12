using Microsoft.Xna.Framework.Graphics;
using MonoGame.Library.Resource;

namespace MonoGame.Library.Graphics;

public static class Materials
{
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
            _sprite ??= new SpriteMaterial ("Sprite", new SpriteEffect (Core.GraphicsDevice), RenderBatchers.Sprite);

            return _sprite;
        }
    }

    private static Material? _canvas;

    public static Material Canvas
    {
        get
        {
            _canvas ??= new SpriteMaterial ("Canvas", new SpriteEffect (Core.GraphicsDevice), RenderBatchers.Sprite, samplerState: SamplerState.PointClamp);

            return _canvas;
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
}
