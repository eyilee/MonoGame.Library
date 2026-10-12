using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public static class Textures
{
    private static Texture2DResource? _pixel;

    public static Texture2DResource Pixel
    {
        get
        {
            if (_pixel == null)
            {
                _pixel = new Texture2DResource ("Pixel", new Texture2D (Core.GraphicsDevice, 1, 1));
                _pixel.Texture.SetData ([Color.White]);
            }

            return _pixel;
        }
    }
}
