using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Eycraft.Framework.Objects;

public class Canvas : Entity, IDisposable
{
    public int Width { get; }

    public int Height { get; }

    public Vector2 Size
    {
        get => _sprite.Size;
        set => _sprite.PreferredSize = value;
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

    public OriginAlignment OriginAlignment
    {
        get => _sprite.OriginAlignment;
        set => _sprite.OriginAlignment = value;
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

    private readonly Texture2D _texture;

    private readonly Texture2DResource _textureResource;

    private readonly Sprite _sprite;

    private readonly Color[] _pixels;

    public Color[] Pixels => _pixels;

    private bool _dirty = true;

    private bool _disposed;

    public Canvas (GraphicsDevice graphicsDevice, string name, int width, int height, int cellSize)
    {
        Width = width;
        Height = height;

        _texture = new Texture2D (graphicsDevice, width, height);
        _textureResource = new Texture2DResource (name, _texture);

        _sprite = AddComponent<Sprite> ();
        _sprite.Region = new TextureRegion (_textureResource, new Rectangle (0, 0, width, height));
        _sprite.PreferredSize = new Vector2 (width * cellSize, height * cellSize);
        _sprite.OriginAlignment = OriginAlignment.Center;

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

    public Vector2 GetPosition (int x, int y) => new (x * Size.X / Width, y * Size.Y / Height);

    public Vector2 GetPosition (int index)
    {
        int x = index % Width;
        int y = index / Width;

        return GetPosition (x, y);
    }

    private int GetIndex (int x, int y) => y * Width + x;

    public void Clear (Color? color)
    {
        Array.Fill (_pixels, color ?? Color.Transparent);

        _dirty = true;
    }

    public override void Draw (RenderManager render)
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
                _texture.Dispose ();
            }

            _disposed = true;
        }
    }
}
