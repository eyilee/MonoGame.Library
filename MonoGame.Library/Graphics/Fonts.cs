using MonoGame.Library.Resource;

namespace MonoGame.Library.Graphics;

public static class Fonts
{
    public static Font Default
    {
        get
        {
            _default ??= Arial12;

            return _default;
        }
    }

    public static Font Arial12
    {
        get
        {
            _arial12 ??= new Font ("Arial12", SpriteFontResource.Arial12.SpriteFont);

            return _arial12;
        }
    }

    private static Font? _default;

    private static Font? _arial12;
}
