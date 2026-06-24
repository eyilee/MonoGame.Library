using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

public abstract class RenderBatcher : INamedResource, IDisposable
{
    public ushort Id { get; }

    public string Name { get; }

    protected readonly GraphicsDevice _graphicsDevice;

    private static readonly ResourceRegistry<RenderBatcher> s_registry = new ();

    private bool _disposed;

    public RenderBatcher (GraphicsDevice graphicsDevice, string name)
    {
        _graphicsDevice = graphicsDevice;

        Id = s_registry.Regist (name, this);
        Name = name;
    }

    ~RenderBatcher () => Dispose (false);

    public abstract void Batch (Mesh mesh);

    public abstract void DrawBatch (MaterialInstance material, MaterialPropertyBlock? properties, Texture? texture);

    public static bool TryGetValue (ushort id, out RenderBatcher? renderBatcher) => s_registry.TryGetValue (id, out renderBatcher);

    public static bool TryGetValue (string name, out RenderBatcher? renderBatcher) => s_registry.TryGetValue (name, out renderBatcher);

    public static ushort GetId (string name) => s_registry.GetId (name);

    public static string GetName (ushort id) => s_registry.GetName (id);

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
            }

            _disposed = true;
        }
    }
}
