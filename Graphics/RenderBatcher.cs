using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

public abstract class RenderBatcher : INamedResource, IDisposable
{
    public ushort Id { get; }

    public string Name { get; }

    protected readonly GraphicsDevice _graphicsDevice;

    private bool _disposed;

    public RenderBatcher (GraphicsDevice graphicsDevice, string name)
    {
        _graphicsDevice = graphicsDevice;

        Id = RenderBatcherRegistry.Regist (name, this);
        Name = name;
    }

    ~RenderBatcher () => Dispose (false);

    public abstract void Batch (Mesh mesh);

    public abstract void DrawBatch (MaterialInstance material, MaterialPropertyBlock? properties, Texture? texture);

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
                RenderBatcherRegistry.UnRegist (this);
            }

            _disposed = true;
        }
    }
}
