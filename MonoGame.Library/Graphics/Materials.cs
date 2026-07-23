using Microsoft.Xna.Framework.Graphics;
using MonoGame.Library.Resource;

namespace MonoGame.Library.Graphics;

public static class Materials
{
    public static Material Sprite
    {
        get
        {
            _sprite ??= new Material ("Sprite", new SpriteEffect (Core.GraphicsDevice), RenderBatchers.Sprite);

            return _sprite;
        }
    }

    public static Material Canvas
    {
        get
        {
            _canvas ??= new Material ("Canvas", new SpriteEffect (Core.GraphicsDevice), RenderBatchers.Sprite, samplerState: SamplerState.PointClamp);

            return _canvas;
        }
    }

    public static SdfMaterial SdfCircle
    {
        get
        {
            _sdfCircle ??= new SdfMaterial ("SdfCircle", EffectResource.SdfCircle.Effect, RenderBatchers.SdfInstance);

            return _sdfCircle;
        }
    }

    public static SdfMaterial SdfLine
    {
        get
        {
            _sdfLine ??= new SdfMaterial ("SdfLine", EffectResource.SdfLine.Effect, RenderBatchers.SdfInstance);

            return _sdfLine;
        }
    }

    public static SdfMaterial SdfParabola
    {
        get
        {
            _sdfParabola ??= new SdfMaterial ("SdfParabola", EffectResource.SdfParabola.Effect, RenderBatchers.SdfInstance);

            return _sdfParabola;
        }
    }

    private static Material? _sprite;

    private static Material? _canvas;

    private static SdfMaterial? _sdfCircle;

    private static SdfMaterial? _sdfLine;

    private static SdfMaterial? _sdfParabola;
}