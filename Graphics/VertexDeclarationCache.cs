using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public class VertexDeclarationCache<T> where T : struct, IVertexType
{
    public static VertexDeclaration VertexDeclaration
    {
        get
        {
            if (s_cache == null)
            {
                s_cache = default (T).VertexDeclaration;
            }

            return s_cache;
        }
    }

    private static VertexDeclaration? s_cache;
}