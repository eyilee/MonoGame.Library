using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

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
}
