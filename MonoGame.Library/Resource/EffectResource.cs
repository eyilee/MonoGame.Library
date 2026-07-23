using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Resource;

internal class EffectResource (string assetName)
{
    public const string SdfCircleName = "SdfCircle.xnb";

    public const string SdfLineName = "SdfLine.xnb";

    public const string SdfParabolaName = "SdfParabola.xnb";

    public static EffectResource SdfCircle
    {
        get
        {
            _sdfCircle ??= new EffectResource (SdfCircleName);

            return _sdfCircle;
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

    private static EffectResource? _sdfCircle;

    private static EffectResource? _sdfLine;

    private static EffectResource? _sdfParabola;

    public Effect Effect { get; } = Core.Resource.Load<Effect> (assetName);
}
