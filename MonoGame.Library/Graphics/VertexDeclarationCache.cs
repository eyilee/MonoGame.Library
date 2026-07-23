using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public class VertexDeclarationCache<T> where T : struct, IVertexType
{
    public static VertexDeclaration VertexDeclaration
    {
        get
        {
            if (_cache == null)
            {
                _cache = default (T).VertexDeclaration;
            }

            return _cache;
        }
    }

    private static VertexDeclaration? _cache;
}