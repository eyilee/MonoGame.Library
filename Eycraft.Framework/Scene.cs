using Eycraft.Framework.Graphics;
using Eycraft.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Eycraft.Framework;

public abstract class Scene : IDisposable
{
    public static GraphicsDevice GraphicsDevice => Core.GraphicsDevice;

    public static RenderManager Render => Core.Render;

    public static InputManager Input => Core.Input;

    protected ContentManager Content { get; }

    private bool _disposed;

    public Scene ()
    {
        Content = new ContentManager (Core.Content.ServiceProvider)
        {
            RootDirectory = Core.Content.RootDirectory
        };
    }

    ~Scene () => Dispose (false);

    public virtual void Initialize ()
    {
        LoadContent ();
    }

    public virtual void LoadContent () { }

    public virtual void UnloadContent ()
    {
        Content.Unload ();
    }

    public virtual void Update (GameTime gameTime) { }

    public virtual void Draw (GameTime gameTime) { }

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
                UnloadContent ();
                Content.Dispose ();
            }

            _disposed = true;
        }
    }
}
