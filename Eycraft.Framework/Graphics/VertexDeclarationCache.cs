using Microsoft.Xna.Framework.Graphics;

namespace Eycraft.Framework.Graphics;

public class VertexDeclarationCache<T> where T : struct, IVertexType
{
    private static VertexDeclaration? _cache;

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
}
