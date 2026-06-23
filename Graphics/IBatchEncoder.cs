using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public interface IBatchEncoder<T> where T : struct, IVertexType
{
    public abstract int VertexCount { get; }

    public abstract void Encode (T[] batchVertices, int index, Mesh mesh);
}
