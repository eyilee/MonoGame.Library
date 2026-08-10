using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Resource;

internal class EffectResource (string assetName)
{
    public const string SdfCircleName = "SdfCircle.xnb";

    public const string SdfFilledCircleName = "SdfFilledCircle.xnb";

    public const string SdfLineName = "SdfLine.xnb";

    public const string SdfParabolaName = "SdfParabola.xnb";

    public const string SdfRectangleName = "SdfRectangle.xnb";

    public const string SdfFilledRectangleName = "SdfFilledRectangle.xnb";

    public static EffectResource SdfCircle
    {
        get
        {
            _sdfCircle ??= new EffectResource (SdfCircleName);

            return _sdfCircle;
        }
    }

    public static EffectResource SdfFilledCircle
    {
        get
        {
            _sdfFilledCircle ??= new EffectResource (SdfFilledCircleName);
            return _sdfFilledCircle;
        }
    }

    public static EffectResource SdfLine
    {
        get
        {
            _sdfLine ??= new EffectResource (SdfLineName);

            return _sdfLine;
        }
    }

    public static EffectResource SdfParabola
    {
        get
        {
            _sdfParabola ??= new EffectResource (SdfParabolaName);

            return _sdfParabola;
        }
    }

    public static EffectResource SdfRectangle
    {
        get
        {
            _sdfRectangle ??= new EffectResource (SdfRectangleName);

            return _sdfRectangle;
        }
    }

    public static EffectResource SdfFilledRectangle
    {
        get
        {
            _sdfFilledRectangle ??= new EffectResource (SdfFilledRectangleName);
            return _sdfFilledRectangle;
        }
    }

    private static EffectResource? _sdfCircle;

    private static EffectResource? _sdfFilledCircle;

    private static EffectResource? _sdfLine;

    private static EffectResource? _sdfParabola;

    private static EffectResource? _sdfRectangle;

    private static EffectResource? _sdfFilledRectangle;

    public Effect Effect { get; } = Core.Resource.Load<Effect> (assetName);
}
