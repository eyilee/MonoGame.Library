using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public interface IBatchEncoder<T> where T : struct, IVertexType
{
    public virtual void Encode (T[] batchVertices, int index, Mesh mesh) { }
}
