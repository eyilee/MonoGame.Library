using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

public class TextureResource : ResourceRegistry<TextureResource>, IResource, IDisposable
{
    public ushort Id { get; }

    public string Name { get; }

    public Texture Texture { get; }

    private bool _disposed;

    public TextureResource (string name, Texture texture)
    {
        Id = Regist (name, this);
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
                UnRegist (this);
                Texture.Dispose ();
            }

            _disposed = true;
        }
    }
}
