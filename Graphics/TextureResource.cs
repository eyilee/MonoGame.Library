using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

public class TextureResource : INamedResource, IDisposable
{
    public ushort Id { get; }

    public string Name { get; }

    public Texture Texture { get; }

    private static readonly ResourceRegistry<TextureResource> s_registry = new ();

    private bool _disposed;

    public TextureResource (string name, Texture texture)
    {
        Id = s_registry.Regist (name, this);
        Name = name;
        Texture = texture;
    }

    ~TextureResource () => Dispose (false);

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
                s_registry.UnRegist (this);
                Texture.Dispose ();
            }

            _disposed = true;
        }
    }
}
