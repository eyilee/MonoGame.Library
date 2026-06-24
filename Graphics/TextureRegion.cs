using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics;

public class TextureRegion (Texture2DResource texture, in Rectangle sourceRectangle)
{
    public Texture2DResource Texture { get; } = texture;

    public Rectangle SourceRectangle { get; } = sourceRectangle;

    public int Width => SourceRectangle.Width;

    public int Height => SourceRectangle.Height;

    public float TopTextureCoordinate => SourceRectangle.Top / (float)Texture.Height;

    public float BottomTextureCoordinate => SourceRectangle.Bottom / (float)Texture.Height;

    public float LeftTextureCoordinate => SourceRectangle.Left / (float)Texture.Width;

    public float RightTextureCoordinate => SourceRectangle.Right / (float)Texture.Width;

    public TextureRegion (Texture2DResource texture, int x, int y, int width, int height)
        : this (texture, new Rectangle (x, y, width, height))
    {
    }
}
