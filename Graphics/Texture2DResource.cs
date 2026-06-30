using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public class Texture2DResource (string name, Texture2D texture) : TextureResource (name, texture)
{
    public new Texture2D Texture { get; } = texture;

    public int Width => Texture.Width;

    public int Height => Texture.Height;

    public float TexelWidth => 1f / Texture.Width;

    public float TexelHeight => 1f / Texture.Height;
}
