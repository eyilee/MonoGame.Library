using MonoGame.Library.Resource;

namespace MonoGame.Library.Graphics;

public static class Fonts
{
    private static Font? _default;

    public static Font Default
    {
        get
        {
            _default ??= Arial12;

            return _default;
        }
    }

    private static Font? _arial12;

    public static Font Arial12
    {
        get
        {
            _arial12 ??= new Font ("Arial12", SpriteFontResource.Arial12.SpriteFont);

            return _arial12;
        }
    }
}
