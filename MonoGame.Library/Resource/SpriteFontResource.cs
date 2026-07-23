using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Resource;

internal class SpriteFontResource (string assetName)
{
    public const string Arial12Name = "Arial12.xnb";

    public static SpriteFontResource Arial12
    {
        get
        {
            _arial12 ??= new SpriteFontResource (Arial12Name);

            return _arial12;
        }
    }

    private static SpriteFontResource? _arial12;

    public SpriteFont SpriteFont { get; } = Core.Resource.Load<SpriteFont> (assetName);
}
