using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public static class RenderBatchers
{
    public static RenderBatcher Standard
    {
        get
        {
            _standard ??= new StandardBatcher<VertexPositionColorTexture> (Core.GraphicsDevice, "Standard", new StandardBatchEncoder ());

            return _standard;
        }
    }

    public static RenderBatcher Sprite
    {
        get
        {
            _sprite ??= new QuadBatcher<VertexPositionColorTexture> (Core.GraphicsDevice, "Sprite", new SpriteBatchEncoder ());

            return _sprite;
        }
    }

    public static RenderBatcher SdfInstance
    {
        get
        {
            _sdfInstance ??= new QuadInstanceBatcher<VertexSdfInstance> (Core.GraphicsDevice, "SdfInstance", new SdfInstanceBatchEncoder ());

            return _sdfInstance;
        }
    }

    private static RenderBatcher? _standard;

    private static RenderBatcher? _sprite;

    private static RenderBatcher? _sdfInstance;
}
