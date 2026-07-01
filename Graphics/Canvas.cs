using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

public class Canvas : IDisposable
{
    public int TextureWidth { get; }

    public int TextureHeight { get; }

    public Vector2 Size
    {
        get => _sprite.Size;
        set => _sprite.Size = value;
    }

    public Vector2 Position
    {
        get => _sprite.Position;
        set => _sprite.Position = value;
    }

    public Color Color
    {
        get => _sprite.Color;
        set => _sprite.Color = value;
    }

    public float Rotation
    {
        get => _sprite.Rotation;
        set => _sprite.Rotation = value;
    }

    public Vector2 Origin
    {
        get => _sprite.Origin;
        set => _sprite.Origin = value;
    }

    public SpriteEffects SpriteEffects
    {
        get => _sprite.SpriteEffects;
        set => _sprite.SpriteEffects = value;
    }

    public float Depth
    {
        get => _sprite.Depth;
        set => _sprite.Depth = value;
    }

    private readonly Texture2DResource _textureResource;

    private readonly Sprite _sprite;

    private readonly Color[] _pixels;

    private bool _dirty = true;

    private bool _disposed;

    public Canvas (GraphicsDevice graphicsDevice, string name, int x, int y, int width, int height, int cellSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero (width, nameof (width));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero (height, nameof (height));

        TextureWidth = width;
        TextureHeight = height;

        _textureResource = new Texture2DResource (name, new (graphicsDevice, width, height, false, SurfaceFormat.Color));

        _sprite = new Sprite (new TextureRegion (_textureResource, new Rectangle (0, 0, width, height)))
        {
            Material = Core.Render.CanvasMaterial,
            Size = new Vector2 (width * cellSize, height * cellSize),
            Position = new Vector2 (x, y),
            Origin = new Vector2 (width * cellSize / 2f, height * cellSize / 2f)
        };

        _pixels = new Color[width * height];
    }

    ~Canvas () => Dispose (false);

    public void SetPixel (int x, int y, Color color)
    {
        _pixels[GetIndex (x, y)] = color;

        _dirty = true;
    }

    public void SetPixel (int index, Color color)
    {
        _pixels[index] = color;

        _dirty = true;
    }

    public Color GetPixel (int x, int y) => _pixels[GetIndex (x, y)];

    public Color GetPixel (int index) => _pixels[index];

    private int GetIndex (int x, int y) => y * TextureWidth + x;

    public void Clear (Color? color)
    {
        Array.Fill (_pixels, color ?? Color.Transparent);

        _dirty = true;
    }

    public void Draw (RenderManager render)
    {
        if (_dirty)
        {
            _textureResource.Texture.SetData (_pixels);
            _dirty = false;
        }

        _sprite.Draw (render);
    }

    public void Dispose ()
    {
        Dispose (true);
        GC.SuppressFinalize (this);
    }

    protected virtual void Dispose (bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _textureResource.Dispose ();
            }

            _disposed = true;
        }
    }
}