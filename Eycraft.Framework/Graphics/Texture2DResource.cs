using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Eycraft.Framework.Graphics;

public class Texture2DResource (string name, Texture2D texture) : TextureResource (name, texture)
{
    public new Texture2D Texture { get; } = texture;

    public int Width => Texture.Width;

    public int Height => Texture.Height;

    public float TexelWidth => 1f / Texture.Width;

    public float TexelHeight => 1f / Texture.Height;

    public static bool TryGetValue (ushort id, out Texture2DResource? texture2D)
    {
        texture2D = TryGetValue (id, out TextureResource? texture) ? texture as Texture2DResource : default;

        return texture2D != null;
    }

    public static bool TryGetValue (string name, out Texture2DResource? texture2D)
    {
        texture2D = TryGetValue (name, out TextureResource? texture) ? texture as Texture2DResource : default;

        return texture2D != null;
    }

    #region Built-in Textures
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
    #endregion
}
